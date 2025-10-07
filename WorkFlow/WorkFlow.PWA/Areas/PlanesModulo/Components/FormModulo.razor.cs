using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class FormModulo : EngramaComponent
	{
		[Parameter] public Modulo ModuloSelected { get; set; }
		[Parameter] public EventCallback<Modulo> OnModuloSaved { get; set; }

		private async Task OnSubmint()
		{
			Loading.Show();
			await OnModuloSaved.InvokeAsync(ModuloSelected);

			Loading.Hide();

		}
	}
}
