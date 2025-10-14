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


		private MudChip<Fases> selectedFaseChip;
		private object selectedPasoValue;

		public ChatLLMComponent chatLLM { get; set; }
		private void OnClickFaseSelected(Fases fase)
		{
			Data.FaseSelected = fase;
		}


		private async Task OnClickPasoSelected(dynamic paso)
		{
			Data.PasoSelected = paso;

			var promptInicial = PrompPasoInicial();
			await chatLLM.ObtenerRespuestaLLM(promptInicial);
			StateHasChanged();
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
	}
}
