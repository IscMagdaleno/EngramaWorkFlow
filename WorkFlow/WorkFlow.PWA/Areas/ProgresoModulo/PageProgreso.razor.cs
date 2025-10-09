using WorkFlow.PWA.Areas.ProgresoModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.PWA.Areas.ProgresoModulo
{
	public partial class PageProgreso : EngramaPage
	{

		public MainProgreso Data { get; set; }

		public bool MostrarProgresoPlan { get; set; }
		protected override void OnInitialized()
		{
			Data = new MainProgreso(httpService, mapperHelper, validaServicioService);
		}

		private void EC_OnProyectoSelecionado(Proyecto proyecto)
		{
			MostrarProgresoPlan = true;


		}

	}
}
