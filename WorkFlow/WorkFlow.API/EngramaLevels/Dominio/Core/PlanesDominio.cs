using EngramaCoreStandar;
using EngramaCoreStandar.Dapper.Results;
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using System.Text.Json;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
using WorkFlow.API.EngramaLevels.Dominio.Servicios.Modelos;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.PlanesModulo;
using WorkFlow.Share.Objetos.PlanesModulo;
using WorkFlow.Share.PostClass.PlanesModulo;

namespace WorkFlow.API.EngramaLevels.Dominio.Core
{
	public class PlanesDominio : IPlanesDominio
	{
		private readonly MapperHelper _mapperHelper;
		private readonly IResponseHelper _responseHelper;
		private readonly IPlanesRepository _planesRepository;
		private readonly IAzureIAService _azureIAService;
		/// <summary>
		/// Initialize the fields receiving the interfaces on the builder
		/// </summary>
		public PlanesDominio(
			MapperHelper mapperHelper,
			IResponseHelper responseHelper,
			IPlanesRepository planesRepository,
			IAzureIAService azureIAService)
		{
			_mapperHelper = mapperHelper;
			_responseHelper = responseHelper;
			_planesRepository = planesRepository;
			_azureIAService = azureIAService;
		}


		public async Task<Response<Planes>> SavePlanes(PostSavePlanes PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostSavePlanes, spSavePlanes.Request>(PostModel);
				var result = await _planesRepository.spSavePlanes(model);
				var validation = _responseHelper.Validacion<spSavePlanes.Result, Planes>(result);
				if (validation.IsSuccess)
				{
					PostModel.iPlanID = validation.Data.iPlanID;
					validation.Data = _mapperHelper.Get<PostSavePlanes, Planes>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<Planes>.BadResult(ex.Message, new());
			}
		}

		public async Task<Response<GenericResponse>> SaveRespuestas(PostSaveRespuestas PostModel)
		{
			try
			{

				List<DTRespuestas> RespuestasList = new List<DTRespuestas>();

				foreach (var item in PostModel.RespuestasList)
				{
					RespuestasList.Add(new DTRespuestas
					{
						dtFechaCreacion = item.dtFechaCreacion,
						iPlanID = item.iPlanID,
						iRespuestaID = item.iRespuestaID,
						nvchPregunta = item.nvchPregunta,
						nvchRespuesta = item.nvchRespuesta,

					});
				}

				var model = new spSaveRespuestas.Request
				{
					RespuestasList = RespuestasList
				};

				var result = await _planesRepository.spSaveRespuestas(model);
				var validation = _responseHelper.Validacion<spSaveRespuestas.Result, GenericResponse>(result);

				return validation;
			}
			catch (Exception ex)
			{
				return Response<GenericResponse>.BadResult(ex.Message, new());
			}
		}


		public async Task<Response<List<Fase>>> GenerateAndSavePlanAsync(PostGeneratePlan PostModel)
		{
			try
			{
				// Consultar título y respuestas desde BD usando iPlanID
				var queryModel = new spGetRespuestasByPlan.Request { iPlanID = PostModel.iPlanID };
				var queryResult = await _planesRepository.spGetRespuestasByPlan(queryModel);

				if (!queryResult.bResult)
				{
					return Response<List<Fase>>.BadResult(queryResult.vchMessage, new());
				}

				string nvchTitulo = queryResult.nvchTitulo;
				var respuestasJson = queryResult.RespuestasJson;
				var preguntasRespuestas = JsonSerializer.Deserialize<List<Respuestas>>(respuestasJson); // Deserializa desde JSON

				// Construir el request para LLM
				var llmRequest = new RequestOpenAI
				{
					Configuration = "Eres un experto en desarrollo de software. Genera un plan de trabajo estructurado en JSON con fases numeradas. Cada fase debe tener subpasos detallados (e.g., base de datos, servicio, vista). Comienza con '1. Creación del proyecto', '2. Configuraciones iniciales', luego funcionalidades específicas basadas en las respuestas.",
					Prompt = $"Título del proyecto: {nvchTitulo}. Preguntas y respuestas: {string.Join("\n", preguntasRespuestas.Select(pr => $"{pr.nvchPregunta}: {pr.nvchRespuesta}"))}. Genera el plan en JSON: {{fases: [{{numero: 1, titulo: '...', descripcion: '...', subpasos: [{{numero: 1.1, detalle: '...'}}]}}]}}"
				};

				// Llamar al LLM
				var llmResponse = await _azureIAService.CallAzureOpenIA(llmRequest);

				// Parsear la respuesta como JSON
				var jsonContent = llmResponse.Content[0].Text;
				var planJson = JsonSerializer.Deserialize<PlanJson>(jsonContent);

				// Mapear a fases y guardar
				var fasesList = new List<DTFases>();
				int faseNumber = 1;
				foreach (var fase in planJson.fases)
				{
					string descripcion = fase.descripcion + "\nSubpasos: " + string.Join("\n", fase.subpasos.Select(s => $"{s.numero}: {s.detalle}"));
					fasesList.Add(new DTFases
					{
						iFaseID = -1,
						iPlanID = PostModel.iPlanID,
						iNumeroFase = faseNumber++,
						nvchTitulo = fase.titulo,
						nvchDescripcion = descripcion,
						nvchEstado = "Pendiente",
						dtFechaCreacion = DateTime.Now,
						dtFechaCompletada = Defaults.SqlMinDate()
					});
				}

				var saveModel = new spSaveFases.Request { FasesList = fasesList };
				var saveResult = await _planesRepository.spSaveFases(saveModel);

				var validation = _responseHelper.Validacion<spSaveFases.Result, List<Fase>>(saveResult);
				if (validation.IsSuccess)
				{
					validation.Data = fasesList.Select(f => _mapperHelper.Get<DTFases, Fase>(f)).ToList();
				}

				return validation;
			}
			catch (Exception ex)
			{
				return Response<List<Fase>>.BadResult(ex.Message, new());
			}
		}
	}


	// Objeto temporal para deserializar JSON del LLM
	internal class PlanJson
	{
		public List<JsonFase> fases { get; set; }
	}

	internal class JsonFase
	{
		public int numero { get; set; }
		public string titulo { get; set; }
		public string descripcion { get; set; }
		public List<JsonSubpaso> subpasos { get; set; }
	}

	internal class JsonSubpaso
	{
		public string numero { get; set; }
		public string detalle { get; set; }
	}

}

