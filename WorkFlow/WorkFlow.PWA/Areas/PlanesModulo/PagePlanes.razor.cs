using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;
using WorkFlow.Share.Objetos.PlanesModulo;

namespace WorkFlow.PWA.Areas.PlanesModulo
{
	public partial class PagePlanes : EngramaPage
	{

		public bool MostrarPreguntas { get; set; }

		public MainPlanes Data { get; set; }

		protected override void OnInitialized()
		{
			Data = new MainPlanes(httpService, mapperHelper, validaServicioService);

			MostrarPreguntas = false;
		}

		private void OnPlanesSaved(Planes planes)
		{
			MostrarPreguntas = true;
		}
		private void OnRespuestasSaved()
		{
			MostrarPreguntas = false;
		}

	}
}
