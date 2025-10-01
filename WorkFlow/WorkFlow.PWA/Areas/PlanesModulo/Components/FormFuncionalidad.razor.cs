using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class FormFuncionalidad : EngramaComponent
	{
		[Parameter] public MainPlanes Data { get; set; }
		[Parameter] public EventCallback<Funcionalidades> OnFuncionalidadesSaved { get; set; }

		private async Task OnSubmint()
		{
			Loading.Show();


			Loading.Hide();

		}
	}
}
