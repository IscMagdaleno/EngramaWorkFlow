using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class GridNuevoPlan : EngramaComponent
	{
		public bool MostrarPreguntas { get; set; }
		public bool MostrarModulos { get; set; }  // Nueva bandera para fases

		[Parameter] public MainPlanes Data { get; set; }
		[Parameter] public EventCallback OnModulosGuardados { get; set; }




		protected override void OnInitialized()
		{
			MostrarPreguntas = false;
			MostrarModulos = false;
		}

		private void OnPlanTrabajoSaved()
		{
			MostrarModulos = true;
		}


		private void OnModuloSelected(Modulo modulo)
		{
			Data.ModuloSelected = modulo;
		}

		private async Task OnClickSaveModulos()
		{


			Loading.Show();

			var result = await Data.SaveAllModulos();
			ShowSnake(result);
			if (result.bResult)
			{

			}
			Loading.Hide();

		}

		public async Task OnFuncionalidadesSaved()
		{
			await Task.Delay(1);
			StateHasChanged();
		}
	}
}
