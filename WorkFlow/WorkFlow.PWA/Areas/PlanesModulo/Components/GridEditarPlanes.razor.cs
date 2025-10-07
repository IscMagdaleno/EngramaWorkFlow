using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class GridEditarPlanes : EngramaComponent
	{
		[Parameter] public MainPlanes Data { get; set; }


		public bool MostrarFuncionalidades { get; set; }
		protected override async Task OnInitializedAsync()
		{
			Loading.Show();

			await Data.PostGetPlanTrabajo(-1);

			Loading.Hide();

		}


		private void OnPlanTrabajoSelected(PlanTrabajo planTrabajo)
		{
			Data.PlanTrabajoSelected = planTrabajo;

			MostrarFuncionalidades = true;
		}

		public async Task OnFuncionalidadesSaved()
		{
			await Task.Delay(1);
			StateHasChanged();
		}
	}
}
