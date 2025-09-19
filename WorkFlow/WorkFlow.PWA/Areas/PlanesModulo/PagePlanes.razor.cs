using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;

namespace WorkFlow.PWA.Areas.PlanesModulo
{
	public partial class PagePlanes : EngramaPage
	{

		public MainPlanes Data { get; set; }

		public bool MostrarNuevoPlan { get; set; }
		protected override void OnInitialized()
		{
			Data = new MainPlanes(httpService, mapperHelper, validaServicioService);
		}


		private void MostrarCrearPlan()
		{
			MostrarNuevoPlan = true;
		}

		private void EscondeCrearPlan()
		{
			MostrarNuevoPlan = false;
		}
	}
}