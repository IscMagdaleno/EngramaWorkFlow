using EngramaCoreStandar.Dapper.Results;
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;
using EngramaCoreStandar.Servicios;

using WorkFlow.Share.Objetos.Planes;
using WorkFlow.Share.Objetos.Proceso;
using WorkFlow.Share.PostClass.Planes;
using WorkFlow.Share.PostClass.Proceso;

namespace WorkFlow.PWA.Areas.ProgresoModulo.Utiles
{
	public class MainProgreso
	{


		private string urlPlanes = @"api/Planes";
		private string urlProceso = @"api/Proceso";

		private readonly IHttpService _httpService;
		private readonly MapperHelper _mapper;
		private readonly IValidaServicioService _validaServicioService;


		public PlanTrabajo PlanTrabajoSelected { get; set; }
		public List<PlanTrabajo> LstPlanTrabajos { get; set; }

		public Proyecto ProyectoSelected { get; set; }
		public List<Proyecto> LstProyectos { get; set; }

		public Fases FaseSelected { get; set; }
		public Paso PasoSelected { get; set; }

		public List<Mensaje> LstMensajes { get; set; }

		public MainProgreso(IHttpService httpService, MapperHelper mapperHelper, IValidaServicioService validaServicioService)
		{
			_httpService = httpService;
			_mapper = mapperHelper;
			_validaServicioService = validaServicioService;

			PlanTrabajoSelected = new PlanTrabajo();
			LstPlanTrabajos = new List<PlanTrabajo>();

			ProyectoSelected = new Proyecto();
			PasoSelected = new Paso();
			FaseSelected = new Fases();
			LstProyectos = new List<Proyecto>();

			LstMensajes = new List<Mensaje>();
		}


		public async Task<SeverityMessage> PostGetPlanTrabajo(int iIdPlanTrabajo)
		{
			var APIUrl = urlPlanes + "/PostGetPlanTrabajo";

			var model = new PostGetPlanTrabajo
			{
				iIdPlanTrabajo = iIdPlanTrabajo
			};

			var response = await _httpService.Post<PostGetPlanTrabajo, Response<List<PlanTrabajo>>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => LstPlanTrabajos = data);
			return validacion;
		}


		public async Task<SeverityMessage> PostGetProyecto()
		{
			var APIUrl = urlPlanes + "/PostGetProyecto";

			var model = new PostGetProyecto { iIdProyecto = -1 };
			var response = await _httpService.Post<PostGetProyecto, Response<List<Proyecto>>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => LstProyectos = data);
			return validacion;
		}

		public async Task<SeverityMessage> PostConversacion(string Mensaje)
		{
			var APIUrl = urlProceso + "/PostConversacion";

			var model = new PostConversacion
			{
				iIdProyecto = ProyectoSelected.iIdProyecto,
				iIdPlanTrabajo = ProyectoSelected.iIdPlanTrabajo,
				iIdFase = FaseSelected.iIdFase,
				nvchContenido = Mensaje
			};
			var response = await _httpService.Post<PostConversacion, Response<Mensaje>>(APIUrl, model);
			var validacion = _validaServicioService.ValidadionServicio(response,
			onSuccess: data => LstMensajes.Add(data));
			return validacion;
		}
	}
}
