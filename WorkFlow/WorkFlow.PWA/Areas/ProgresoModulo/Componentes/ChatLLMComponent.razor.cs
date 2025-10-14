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
	public partial class ChatLLMComponent : EngramaComponent
	{
		[Inject] public IJSRuntime JSRuntime { get; set; }
		[Parameter] public MainProgreso Data { get; set; }


		private string mensajeActual = "";

		private bool esperandoRespuesta = false;
		private ElementReference chatContainer;



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




		private bool EsUsuario(Mensaje mensaje)
		{
			return (mensaje.nvchRol == "user");
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{

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


			esperandoRespuesta = true;

			await ObtenerRespuestaLLM(mensajeActual);


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

		public async Task ObtenerRespuestaLLM(string mensaje)
		{
			Loading.Show();
			try
			{
				var result = await Data.PostConversacion(mensaje);
				ShowSnake(result);
				if (result.bResult)
				{
					StateHasChanged();
					await Task.Delay(1);
				}


			}
			catch (Exception ex)
			{
				Snackbar.Add($"Error al comunicarse con el asistente: {ex.Message}", Severity.Error);
			}

			Loading.Hide();

		}

		private void UsarSugerencia(string sugerencia)
		{
			mensajeActual = sugerencia;
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
			return "max-width: 100%; background-color: white; border-radius: 18px 18px 18px 4px;";
		}



	}
}
