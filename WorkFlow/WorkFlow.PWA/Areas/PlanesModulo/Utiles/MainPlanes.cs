using EngramaCoreStandar.Dapper.Results;
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;
using EngramaCoreStandar.Servicios;

using WorkFlow.Share.Objetos.PlanesModulo;

using WorkFlow.Share.PostClass.PlanesModulo;

namespace WorkFlow.PWA.Areas.PlanesModulo.Utiles
{
	public class MainPlanes
	{
		private string url = @"api/Planes";

		private readonly IHttpService _httpService;
		private readonly MapperHelper _mapper;
		private readonly IValidaServicioService _validaServicioService;

		public Planes PlanesSelected { get; set; }
		public List<Respuestas> LstRespuestas { get; set; }


		public MainPlanes(IHttpService httpService, MapperHelper mapperHelper, IValidaServicioService validaServicioService)
		{
			_httpService = httpService;
			_mapper = mapperHelper;
			_validaServicioService = validaServicioService;

			PlanesSelected = new Planes();
			LstRespuestas = new List<Respuestas>();
		}
		public async Task<SeverityMessage> PostSavePlanes()
		{
			PlanesSelected.iUsuarioID = 1;
			PlanesSelected.nvchEstado = "Pendiente";

			var APIUrl = url + "/PostSavePlanes";
			var model = _mapper.Get<Planes, PostSavePlanes>(PlanesSelected);
			var response = await _httpService.Post<PostSavePlanes, Response<Planes>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => PlanesSelected.iPlanID = (data.iPlanID));
			return validacion;
		}


		public async Task<SeverityMessage> PostSaveRespuestas()
		{
			var APIUrl = url + "/PostSaveRespuestas";
			var model = new PostSaveRespuestas()
			{
				RespuestasList = LstRespuestas
			};

			var response = await _httpService.Post<PostSaveRespuestas, Response<Respuestas>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response);
			return validacion;

		}



	}
}
