using Microsoft.AspNetCore.Components;

using MudBlazor;

using WorkFlow.PWA.Areas.ProgresoModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Componentes
{
	public partial class ProgresoProyeto : EngramaComponent
	{


		[Parameter]
		public MainProgreso Data { get; set; }


		private void OnClickFaseSelected(Fases fase)
		{
			Data.FaseSelected = fase;
		}

		private MudChip<Fases> selectedFaseChip;
		private object selectedPasoValue;

		private void OnClickFaseSelected(dynamic fase)
		{
			Data.FaseSelected = fase;
			Data.PasoSelected = new Paso(); // Reiniciar paso seleccionado
			selectedPasoValue = null;
			StateHasChanged();
		}

		private void OnClickPasoSelected(dynamic paso)
		{
			Data.PasoSelected = paso;
			StateHasChanged();
		}

		private string GetTruncatedText(string text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
				return "N/A";

			return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
		}

	}
}
