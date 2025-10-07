using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class ConsultaProyectos : EngramaComponent
	{

		[Parameter] public MainPlanes Data { get; set; }


		public bool MostrarProyecto { get; set; }
		protected override async Task OnInitializedAsync()
		{
			Loading.Show();

			var result = await Data.PostGetProyecto();

			Loading.Hide();

		}

		private async Task OnProyectoSelected(Proyecto proyecto)
		{
			Loading.Show();
			Data.ProyectoSelected = proyecto;

			var result = await Data.PostGetPlanTrabajo(proyecto.iIdPlanTrabajo);
			if (result.bResult)
			{

				Data.PlanTrabajoSelected = Data.LstPlanTrabajos.SingleOrDefault();
				MostrarProyecto = true;
			}
			Loading.Hide();

		}
	}
}
