using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Servicios;

namespace WorkFlow.PWA.Areas.PlanesModulo.Utiles
{
	public class MainPlanes
	{
		private string url = @"api/Planes";

		private readonly IHttpService _httpService;
		private readonly MapperHelper _mapper;
		private readonly IValidaServicioService _validaServicioService;


		public MainPlanes(IHttpService httpService, MapperHelper mapperHelper, IValidaServicioService validaServicioService)
		{
			_httpService = httpService;
			_mapper = mapperHelper;
			_validaServicioService = validaServicioService;

		}

	}
}
