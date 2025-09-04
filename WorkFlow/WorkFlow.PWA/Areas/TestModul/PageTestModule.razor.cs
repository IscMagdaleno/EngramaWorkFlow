using WorkFlow.PWA.Areas.TestModul.Utiles;
using WorkFlow.PWA.Shared.Common;

namespace WorkFlow.PWA.Areas.TestModul
{
	public partial class PageTestModule : EngramaPage
	{


		public DataTest Data { get; set; }

		protected override void OnInitialized()
		{
			Data = new DataTest(httpService, mapperHelper, validaServicioService);
		}

		protected override async Task OnInitializedAsync()
		{
		}



	}
}
