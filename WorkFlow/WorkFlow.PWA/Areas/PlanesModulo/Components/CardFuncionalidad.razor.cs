using EngramaCoreStandar.Extensions;

using Microsoft.AspNetCore.Components;

using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Components
{
	public partial class CardFuncionalidad
	{
		[Parameter]
		public Funcionalidades? Funcionalidad { get; set; }
		[Parameter] public EventCallback<Funcionalidades> OnFuncionalidadSelected { get; set; }
		[Parameter] public bool MostrarSeleccionar { get; set; }


		public bool MostrarFormFuncionalidad { get; set; }


		private void OnClickFormModulo()
		{
			MostrarFormFuncionalidad = MostrarFormFuncionalidad.False();
		}
		private async Task OnFuncionalidadesSaved()
		{
			MostrarFormFuncionalidad = false;
			await OnFuncionalidadSelected.InvokeAsync(Funcionalidad);
		}
	}
}
