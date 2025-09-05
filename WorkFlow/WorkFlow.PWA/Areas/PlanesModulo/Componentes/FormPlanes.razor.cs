using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.PlanesModulo;

namespace WorkFlow.PWA.Areas.PlanesModulo.Componentes
{
	public partial class FormPlanes : EngramaComponent
	{
		[Parameter] public MainPlanes Data { get; set; }
		[Parameter] public EventCallback<Planes> OnPlanesSaved { get; set; }

		private async Task OnSubmint()
		{
			Loading.Show();

			var result = await Data.PostSavePlanes();
			ShowSnake(result);
			if (result.bResult)
			{
				await OnPlanesSaved.InvokeAsync(Data.PlanesSelected);
			}
			Loading.Hide();

		}



	}
}
