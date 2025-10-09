using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.ProgresoModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Componentes
{
	public partial class ConsultaPlanesProgreso : EngramaComponent
	{

		[Parameter] public MainProgreso Data { get; set; }

		[Parameter] public EventCallback<Proyecto> EC_OnProyectoSelecionado { get; set; }

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

				await EC_OnProyectoSelecionado.InvokeAsync(proyecto);
			}
			Loading.Hide();

		}
	}
}
