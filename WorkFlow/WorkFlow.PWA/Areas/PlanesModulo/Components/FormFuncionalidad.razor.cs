using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class FormFuncionalidad : EngramaComponent
	{
		[Parameter] public Funcionalidades FuncionalidaSelected { get; set; }
		[Parameter] public EventCallback<Funcionalidades> OnFuncionalidadesSaved { get; set; }

		private async Task OnSubmint()
		{
			await OnFuncionalidadesSaved.InvokeAsync(FuncionalidaSelected);

		}
	}
}
