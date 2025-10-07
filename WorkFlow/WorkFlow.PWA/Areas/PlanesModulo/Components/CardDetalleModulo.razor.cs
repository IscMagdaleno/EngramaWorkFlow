using EngramaCoreStandar.Extensions;

using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class CardDetalleModulo : EngramaComponent
	{


		[Parameter] public Modulo? ModuloSelected { get; set; }

		[Parameter] public EventCallback<Modulo> OnInfoSaved { get; set; }

		public bool MostrarFormModulo { get; set; }


		private void OnClickFormModulo()
		{
			MostrarFormModulo = MostrarFormModulo.False();
		}

		private async Task OnModuloSaved(Modulo modulo)
		{
			MostrarFormModulo = false;
			await OnInfoSaved.InvokeAsync(modulo);
		}

		private async Task OnFuncionalidadSelected(Funcionalidades funcionalidades)
		{
			await OnInfoSaved.InvokeAsync(ModuloSelected);
		}

	}
}
