using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.ProgresoModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Componentes
{
	public partial class GridPlanes : EngramaComponent
	{

		[Parameter] public MainProgreso Data { get; set; }
		[Parameter] public EventCallback OnPlanSelected { get; set; }

		protected override async Task OnInitializedAsync()
		{
			Loading.Show();

			await Data.PostGetPlanTrabajo();

			Loading.Hide();

		}


		private async Task OnPlanTrabajoSelected(PlanTrabajo planTrabajo)
		{
			Data.PlanTrabajoSelected = planTrabajo;

			await OnPlanSelected.InvokeAsync();
		}
	}
}
