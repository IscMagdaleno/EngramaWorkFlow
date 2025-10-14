using EngramaCoreStandar.Extensions;

using Markdig;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using MudBlazor;

using WorkFlow.PWA.Areas.ProgresoModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Proceso;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Componentes
{
	public partial class ChatPaginadoComponent : EngramaComponent
	{
		[Inject] public IJSRuntime JSRuntime { get; set; }
		[Parameter] public MainProgreso Data { get; set; }

		private string nuevoMensaje = "";
		private bool esperandoRespuesta = false;
		private ElementReference conversacionContainer;
		private static MarkdownPipeline pipeline;

		// Paginación
		private int indicePaginaActual = 0;
		private List<ConversacionPagina> conversacionesPaginadas = new();
		private ConversacionPagina conversacionActual =>
			conversacionesPaginadas.ElementAtOrDefault(indicePaginaActual);
		private int totalConversaciones => conversacionesPaginadas.Count;

		// Clase helper para agrupar pregunta-respuesta


		protected override void OnInitialized()
		{
			if (pipeline == null)
			{
				pipeline = new MarkdownPipelineBuilder()
					.UseAdvancedExtensions()
					.UseSoftlineBreakAsHardlineBreak()
					.Build();
			}

			if (Data?.LstMensajes == null || !Data.LstMensajes.Any())
			{
				Snackbar.Add("No hay mensajes disponibles para mostrar.", Severity.Info);
				return;
			}

			OrganizarConversacionesPaginadas();
		}

		protected override void OnParametersSet()
		{
			OrganizarConversacionesPaginadas();
		}

		private void OrganizarConversacionesPaginadas()
		{
			conversacionesPaginadas.Clear();

			if (Data?.LstMensajes == null || !Data.LstMensajes.Any())
				return;

			var mensajesOrdenados = Data.LstMensajes.OrderBy(m => m.dtFecha).ToList();

			for (int i = 0; i < mensajesOrdenados.Count; i++)
			{
				var mensaje = mensajesOrdenados[i];

				if (mensaje.nvchRol != "user")
					continue;

				var conversacion = new ConversacionPagina
				{
					MensajeUsuario = mensaje,
					MensajeAsistente = null
				};

				if (i + 1 < mensajesOrdenados.Count &&
					mensajesOrdenados[i + 1].nvchRol == "assistant")
				{
					conversacion.MensajeAsistente = mensajesOrdenados[i + 1];
					i++;
				}

				conversacionesPaginadas.Add(conversacion);
			}

			if (conversacionesPaginadas.Any())
			{
				indicePaginaActual = conversacionesPaginadas.Count - 1;
			}
		}



		public async Task IniciarConversacion(string mensajeInicial)
		{
			nuevoMensaje = mensajeInicial;
			await EnviarNuevoMensaje();
		}

		private async Task EnviarNuevoMensaje()
		{
			if (string.IsNullOrWhiteSpace(nuevoMensaje))
				return;

			var mensajeParaEnviar = nuevoMensaje;
			nuevoMensaje = "";
			esperandoRespuesta = true;

			var MensajeUsuario = new Mensaje
			{
				nvchContenido = mensajeParaEnviar,
				nvchRol = "user",
				dtFecha = DateTime.Now
			};

			Data.LstMensajes.Add(MensajeUsuario);

			var nuevaConversacion = new ConversacionPagina
			{
				MensajeUsuario = MensajeUsuario,
				MensajeAsistente = null
			};

			conversacionesPaginadas.Add(nuevaConversacion);
			indicePaginaActual = conversacionesPaginadas.Count - 1;

			await ObtenerRespuestaLLM(mensajeParaEnviar);
			esperandoRespuesta = false;
			OrganizarConversacionesPaginadas();
		}

		private async Task ObtenerRespuestaLLM(string mensaje)
		{
			Loading.Show();
			try
			{
				var result = await Data.PostConversacion(mensaje);
				if (result.bResult.False())
				{
					Snackbar.Add("No se recibió una respuesta válida del asistente.", Severity.Error);
					return;
				}
				ShowSnake(result);

				indicePaginaActual = conversacionesPaginadas.Count - 1;
			}
			catch (Exception ex)
			{
				Snackbar.Add($"Error al comunicarse con el asistente: {ex.Message}", Severity.Error);
			}
			finally
			{
				Loading.Hide();
			}
		}

		private void ConversacionAnterior()
		{
			if (indicePaginaActual > 0)
			{
				indicePaginaActual--;
			}
		}

		private void ConversacionSiguiente()
		{
			if (indicePaginaActual < totalConversaciones - 1)
			{
				indicePaginaActual++;
			}
		}

		private void UsarSugerencia(string sugerencia)
		{
			nuevoMensaje = sugerencia;
		}

		private async Task HandleKeyDown(KeyboardEventArgs e)
		{
			if (e.Key == "Enter" && e.CtrlKey && !string.IsNullOrWhiteSpace(nuevoMensaje))
			{
				await EnviarNuevoMensaje();
			}
		}

		private string ConvertirMarkdownAHtml(string markdown)
		{
			if (string.IsNullOrWhiteSpace(markdown))
				return string.Empty;

			try
			{
				return Markdown.ToHtml(markdown, pipeline);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al convertir Markdown: {ex.Message}");
				return $"<p>{markdown}</p>";
			}
		}

		private async Task CopiarAlPortapapeles(string texto)
		{
			try
			{
				await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", texto);
				Snackbar.Add("Contenido copiado al portapapeles", Severity.Success);
			}
			catch
			{
				Snackbar.Add("No se pudo copiar al portapapeles", Severity.Warning);
			}
		}
	}
}