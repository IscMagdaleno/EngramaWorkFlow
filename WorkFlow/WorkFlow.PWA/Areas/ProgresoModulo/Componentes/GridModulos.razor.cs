using Microsoft.AspNetCore.Components;

using WorkFlow.PWA.Areas.ProgresoModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Componentes
{
	public partial class GridModulos : EngramaComponent
	{
		[Parameter] public MainProgreso Data { get; set; }



		private void OnModuloSelected(Modulo modulo)
		{
			Data.ModuloSelected = modulo;
		}

		private void OnFuncionalidadSelected(Funcionalidades funcionalidad)
		{
			Data.FuncionalidadSelected = funcionalidad;
		}




	}
}
