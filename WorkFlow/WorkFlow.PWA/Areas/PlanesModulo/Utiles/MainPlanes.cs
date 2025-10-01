using EngramaCoreStandar.Dapper.Results;
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;
using EngramaCoreStandar.Servicios;

using WorkFlow.Share.Objetos.Planes;
using WorkFlow.Share.PostClass.Planes;

namespace WorkFlow.PWA.Areas.PlanesModulo.Utiles
{
	public class MainPlanes
	{
		private string url = @"api/Planes";

		private readonly IHttpService _httpService;
		private readonly MapperHelper _mapper;
		private readonly IValidaServicioService _validaServicioService;


		public PlanTrabajo PlanTrabajoSelected { get; set; }
		public List<PlanTrabajo> LstPlanTrabajos { get; set; }

		public Modulo ModuloSelected { get; set; }
		public MainPlanes(IHttpService httpService, MapperHelper mapperHelper, IValidaServicioService validaServicioService)
		{
			_httpService = httpService;
			_mapper = mapperHelper;
			_validaServicioService = validaServicioService;

			PlanTrabajoSelected = new PlanTrabajo();
			LstPlanTrabajos = new List<PlanTrabajo>();
			ModuloSelected = new Modulo();
		}


		public async Task<SeverityMessage> PostSavePlanTrabajo()
		{

			var APIUrl = url + "/PostSavePlanTrabajo";
			var model = _mapper.Get<PlanTrabajo, PostSavePlanTrabajo>(PlanTrabajoSelected);
			var response = await _httpService.Post<PostSavePlanTrabajo, Response<PlanTrabajo>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => PlanTrabajoSelected = (data));
			return validacion;

		}



		public async Task<SeverityMessage> PostGetPlanTrabajo()
		{
			var APIUrl = url + "/PostGetPlanTrabajo";

			var model = _mapper.Get<PlanTrabajo, PostGetPlanTrabajo>(PlanTrabajoSelected);
			var response = await _httpService.Post<PostGetPlanTrabajo, Response<List<PlanTrabajo>>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => LstPlanTrabajos = data);
			return validacion;
		}


		public async Task<SeverityMessage> SaveAllModulos()
		{
			var result = new SeverityMessage(false, "Proceso de guardar módulos");
			foreach (var item in PlanTrabajoSelected.LstModulos)
			{
				ModuloSelected = item;
				ModuloSelected.iIdPlanTrabajo = PlanTrabajoSelected.iIdPlanTrabajo;
				result = await PostSaveModulo();
			}
			return result;

		}


		public async Task<SeverityMessage> PostSaveModulo()
		{
			var APIUrl = url + "/PostSaveModulo";
			var model = new PostSaveModulo { Modulo = ModuloSelected };
			var response = await _httpService.Post<PostSaveModulo, Response<Modulo>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => ModuloSelected = (data));
			return validacion;

		}


	}
}
