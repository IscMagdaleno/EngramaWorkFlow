using Markdig;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using MudBlazor;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Componentes
{
	public partial class ChatLLMComponent
	{
		[Inject] public IJSRuntime JSRuntime { get; set; }
		[Parameter] public dynamic Paso { get; set; }
		[Parameter] public dynamic Fase { get; set; }
		[Parameter] public dynamic Proyecto { get; set; }

		private List<MensajeChat> mensajes = new();
		private string mensajeActual = "";
		private bool esperandoRespuesta = false;
		private ElementReference chatContainer;

		public class MensajeChat
		{
			public string Contenido { get; set; }
			public bool EsUsuario { get; set; }
			public DateTime Timestamp { get; set; }
		}

		private static MarkdownPipeline pipeline;

		protected override void OnInitialized()
		{
			// Configurar el pipeline de Markdown
			if (pipeline == null)
			{
				pipeline = new MarkdownPipelineBuilder()
					.UseAdvancedExtensions()
					.UseSoftlineBreakAsHardlineBreak()
					.Build();
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				// Mensaje de bienvenida del asistente
				await Task.Delay(500);
				mensajes.Add(new MensajeChat
				{
					Contenido = $"¡Hola! Estoy aquí para ayudarte con el Paso {Paso.smNumeroSecuencia}.\n\n" +
								$"Descripción: {Paso.nvchDescripcion}\n\n" +
								$"¿En qué puedo asistirte?",
					EsUsuario = false,
					Timestamp = DateTime.Now
				});
				StateHasChanged();
			}

			// Hacer scroll al final después de cada render
			try
			{
				await JSRuntime.InvokeVoidAsync("scrollToBottom", chatContainer);
			}
			catch
			{
				// Ignorar errores de JS durante prerendering
			}
		}

		private async Task EnviarMensaje()
		{
			if (string.IsNullOrWhiteSpace(mensajeActual)) return;

			// Agregar mensaje del usuario
			mensajes.Add(new MensajeChat
			{
				Contenido = mensajeActual,
				EsUsuario = true,
				Timestamp = DateTime.Now
			});

			var mensajeEnviado = mensajeActual;
			mensajeActual = "";
			esperandoRespuesta = true;
			StateHasChanged();

			// Simular respuesta del LLM (aquí debes integrar tu API real)
			await Task.Delay(1500);

			// TODO: Aquí debe ir la llamada real a tu servicio LLM
			var respuestaLLM = await ObtenerRespuestaLLM(mensajeEnviado);

			mensajes.Add(new MensajeChat
			{
				Contenido = respuestaLLM,
				EsUsuario = false,
				Timestamp = DateTime.Now
			});

			esperandoRespuesta = false;
			StateHasChanged();
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

		private async Task<string> ObtenerRespuestaLLM(string mensaje)
		{
			try
			{
				// Construir el contexto del proyecto
				//				var contexto = $@"Proyecto: {Proyecto.nvchNombre}
				//Fase: {Fase.nvchTitulo}
				//Paso {Paso.smNumeroSecuencia}: {Paso.nvchDescripcion}
				//Propósito: {Paso.nvchProposito}
				//Características: {Paso.nvchCaracteristicas}
				//Enfoque: {Paso.nvchEnfoque}";

				//				// Convertir mensajes previos al formato del servicio
				//				var historial = mensajes
				//					.Where(m => !string.IsNullOrEmpty(m.Contenido))
				//					.Select(m => new MensajeHistorial
				//					{
				//						Rol = m.EsUsuario ? "user" : "assistant",
				//						Contenido = m.Contenido
				//					})
				//					.ToList();

				//				// Llamar al servicio LLM
				//				//var respuesta = await LLMService.EnviarMensajeAsync(contexto, mensaje, historial);
				//				return respuesta;

				return mensaje;
			}
			catch (Exception ex)
			{
				Snackbar.Add($"Error al comunicarse con el asistente: {ex.Message}", Severity.Error);
				return "Lo siento, no pude procesar tu mensaje. Por favor, intenta de nuevo.";
			}
		}

		private void UsarSugerencia(string sugerencia)
		{
			mensajeActual = sugerencia;
			StateHasChanged();
		}

		private void LimpiarChat()
		{
			mensajes.Clear();
			mensajeActual = "";
			esperandoRespuesta = false;
			StateHasChanged();
		}

		private async Task HandleKeyDown(KeyboardEventArgs e)
		{
			if (e.Key == "Enter" && !e.ShiftKey && !string.IsNullOrWhiteSpace(mensajeActual))
			{
				await EnviarMensaje();
			}
		}

		private string GetMessageClass(bool esUsuario)
		{
			return esUsuario ? "d-flex justify-end" : "d-flex justify-start";
		}

		private string GetMessageStyle(bool esUsuario)
		{
			if (esUsuario)
			{
				return "max-width: 70%; background-color: var(--mud-palette-primary); color: white; border-radius: 18px 18px 4px 18px;";
			}
			return "max-width: 70%; background-color: white; border-radius: 18px 18px 18px 4px;";
		}
	}
}
