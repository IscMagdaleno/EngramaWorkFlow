using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class FormPlanes : EngramaComponent
	{

		[Parameter] public MainPlanes Data { get; set; }
		[Parameter] public EventCallback<PlanTrabajo> OnPlanTrabajoSaved { get; set; }



		private async Task OnSubmint()
		{
			Loading.Show();

			var result = await Data.PostSavePlanTrabajo();
			ShowSnake(result);
			if (result.bResult)
			{
				await OnPlanTrabajoSaved.InvokeAsync(Data.PlanTrabajoSelected);
			}
			Loading.Hide();
		}
	}
}
