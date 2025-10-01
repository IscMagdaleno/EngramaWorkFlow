using EngramaCoreStandar.Dapper.Results;
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;
using EngramaCoreStandar.Servicios;

using WorkFlow.Share.Objetos.Planes;
using WorkFlow.Share.PostClass.Planes;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Utiles
{
	public class MainProgreso
	{


		private string urlPlanes = @"api/Planes";

		private readonly IHttpService _httpService;
		private readonly MapperHelper _mapper;
		private readonly IValidaServicioService _validaServicioService;


		public PlanTrabajo PlanTrabajoSelected { get; set; }
		public List<PlanTrabajo> LstPlanTrabajos { get; set; }

		public Modulo ModuloSelected { get; set; }
		public MainProgreso(IHttpService httpService, MapperHelper mapperHelper, IValidaServicioService validaServicioService)
		{
			_httpService = httpService;
			_mapper = mapperHelper;
			_validaServicioService = validaServicioService;

			PlanTrabajoSelected = new PlanTrabajo();
			LstPlanTrabajos = new List<PlanTrabajo>();
			ModuloSelected = new Modulo();
		}

		public async Task<SeverityMessage> PostGetPlanTrabajo()
		{
			var APIUrl = urlPlanes + "/PostGetPlanTrabajo";

			var model = _mapper.Get<PlanTrabajo, PostGetPlanTrabajo>(PlanTrabajoSelected);
			var response = await _httpService.Post<PostGetPlanTrabajo, Response<List<PlanTrabajo>>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => LstPlanTrabajos = data);
			return validacion;
		}

	}
}
