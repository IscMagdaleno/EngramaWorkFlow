using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class GridNuevoPlan : EngramaComponent
	{
		public bool MostrarPreguntas { get; set; }
		public bool MostrarFuncionalidades { get; set; }  // Nueva bandera para fases

		[Parameter] public MainPlanes Data { get; set; }




		protected override void OnInitialized()
		{
			MostrarPreguntas = false;
			MostrarFuncionalidades = false;
		}

		private void OnPlanTrabajoSaved()
		{
			MostrarFuncionalidades = true;
		}

		public async Task OnFuncionalidadesSaved()
		{
			await Task.Delay(1);
			StateHasChanged();
		}
	}
}
