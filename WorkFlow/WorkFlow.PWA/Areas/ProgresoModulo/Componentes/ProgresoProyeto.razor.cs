using Microsoft.AspNetCore.Components;

using MudBlazor;

using System.Text;

using WorkFlow.PWA.Areas.ProgresoModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Componentes
{
	public partial class ProgresoProyeto : EngramaComponent
	{


		[Parameter] public MainProgreso Data { get; set; }

		private MudStepper stepper;
		private bool mostrarDetallesPaso;
		private ChatPaginadoComponent chatPaginado;
		private List<BreadcrumbItem> _breadcrumbItems;


		public ChatLLMComponent chatLLM { get; set; }


		private void OnClickFaseSelected(Fases fase)
		{
			Data.FaseSelected = fase;
		}


		private string GetTruncatedText(string text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
				return "N/A";

			return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
		}


		private string PrompPasoInicial()
		{
			var paso = Data.PasoSelected;
			var promptInicial = new StringBuilder();
			promptInicial.AppendLine("Ayúdame a realizar el siguiente paso");
			promptInicial.AppendLine($"Paso en secuencia numero: {paso.smNumeroSecuencia}.");
			promptInicial.AppendLine($"Descripción: {paso.nvchDescripcion}.");
			promptInicial.AppendLine($"Propósito: {paso.nvchProposito}.");
			promptInicial.AppendLine($"Características: {paso.nvchCaracteristicas}.");
			promptInicial.AppendLine($"Enfoque: {paso.nvchEnfoque}.");

			promptInicial.AppendLine($"Dame las clases que tengo que crear," +
			$" las tablas, las configuraciones bien definidas, con comentarios de para que sirve cada cosa.");

			return promptInicial.ToString();
		}



		private async Task OnClickPasoSelected(Paso paso)
		{
			Data.PasoSelected = paso;
			StateHasChanged();
			await Task.Delay(1);

			var iniciarMensje = PrompPasoInicial();
			await chatPaginado.IniciarConversacion(iniciarMensje);

		}


		private async Task ReiniciarConversacion()
		{
			// Implement your logic to reset conversation
		}
	}
}
