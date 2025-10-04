using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using System.Text.Json;
using System.Text.RegularExpressions;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
using WorkFlow.API.EngramaLevels.Dominio.Servicios.Modelos;
using WorkFlow.API.EngramaLevels.Dominio.Servicios.Utiles;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.Planes;
using WorkFlow.Share.Objetos.Planes;
using WorkFlow.Share.PostClass.Planes;

namespace WorkFlow.API.EngramaLevels.Dominio.Core
{
	public class PlanesDominio : IPlanesDominio
	{
		private readonly MapperHelper _mapperHelper;
		private readonly IResponseHelper _responseHelper;
		private readonly IPlanesRepository _planesRepository;
		private readonly IAzureIAService _azureIAService;
		private readonly ILLMModuleGenerator _llmModuleGenerator;

		/// <summary>
		/// Initialize the fields receiving the interfaces on the builder
		/// </summary>
		public PlanesDominio(
			MapperHelper mapperHelper,
			IResponseHelper responseHelper,
			IPlanesRepository planesRepository,
			IAzureIAService azureIAService,
			ILLMModuleGenerator lLMModuleGenerator)
		{
			_mapperHelper = mapperHelper;
			_responseHelper = responseHelper;
			_planesRepository = planesRepository;
			_azureIAService = azureIAService;
			_llmModuleGenerator = lLMModuleGenerator;
		}



		public async Task<Response<PlanTrabajo>> SavePlanTrabajo(PostSavePlanTrabajo PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostSavePlanTrabajo, spSavePlanTrabajo.Request>(PostModel);
				var result = await _planesRepository.spSavePlanTrabajo(model);
				var validation = _responseHelper.Validacion<spSavePlanTrabajo.Result, PlanTrabajo>(result);
				if (validation.IsSuccess)
				{

					var ResultModules = await GenerateModulesFromPlan(validation.Data.iIdPlanTrabajo);
					if (ResultModules.IsSuccess)
					{
						validation.Data = ResultModules.Data;
					}

				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<PlanTrabajo>.BadResult(ex.Message, new());
			}
		}

		public async Task<Response<IEnumerable<PlanTrabajo>>> GetPlanTrabajo(PostGetPlanTrabajo PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostGetPlanTrabajo, spGetPlanTrabajo.Request>(PostModel);
				var result = await _planesRepository.spGetPlanTrabajo(model);
				var validation = _responseHelper.Validacion<spGetPlanTrabajo.Result, PlanTrabajo>(result);
				if (validation.IsSuccess)
				{
					foreach (var plan in validation.Data)
					{

						var moduloModel = new spGetModulo.Request
						{
							iIdPlanTrabajo = plan.iIdPlanTrabajo
						};

						var resultModulo = await _planesRepository.spGetModulo(moduloModel);
						var validationModulo = _responseHelper.Validacion<spGetModulo.Result, Modulo>(resultModulo);

						if (validationModulo.IsSuccess)
						{

							foreach (var modulo in validationModulo.Data)
							{


								var funcionalidadModel = new spGetFuncionalidad.Request
								{
									iIdModulo = modulo.iIdModulo
								};

								var resultFuncionalidades = await _planesRepository.spGetFuncionalidad(funcionalidadModel);
								var validationfuncialidades = _responseHelper.Validacion<spGetFuncionalidad.Result, Funcionalidades>(resultFuncionalidades);

								if (validationfuncialidades.IsSuccess)
								{


									modulo.LstFuncionalidades = validationfuncialidades.Data.ToList();

								}

							}

							plan.LstModulos = validationModulo.Data.ToList();
						}

					}

				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<IEnumerable<PlanTrabajo>>.BadResult(ex.Message, new List<PlanTrabajo>());
			}
		}


		public async Task<Response<PlanTrabajo>> GenerateModulesFromPlan(int iIdPlanTrabajo)
		{
			var respuesta = new Response<PlanTrabajo>();
			respuesta.Data = new PlanTrabajo();
			respuesta.IsSuccess = false;
			try
			{
				// Reutilizamos el método existente para obtener el plan por ID
				var postModel = new PostGetPlanTrabajo { iIdPlanTrabajo = iIdPlanTrabajo };
				var planResponse = await GetPlanTrabajo(postModel);
				if (planResponse.IsSuccess)
				{

					respuesta.Data = planResponse.Data.FirstOrDefault();

					// Asumimos que PlanTrabajo tiene propiedades como sTitulo y sDescripcion basadas en convenciones comunes
					// Si los nombres son diferentes, ajusta según tu entidad PlanTrabajo
					var titulo = SanitizeInput(respuesta.Data.vchNombre ?? string.Empty);
					var descripcion = SanitizeInput(respuesta.Data.nvchDescripcion ?? string.Empty);

					// Llamamos a la nueva clase para generar el prompt y consultar el LLM
					var Modules = await _llmModuleGenerator.GenerateModules(titulo, descripcion);

					respuesta.Data.LstModulos = Modules;
					respuesta.IsSuccess = true;
				}

				return respuesta;
			}
			catch (Exception ex)
			{
				return Response<PlanTrabajo>.BadResult(ex.Message, new PlanTrabajo());
			}
		}


		public async Task<Response<Proyecto>> GeneraFasesDesarrollo(PostGeneraFasesDesarrollo PostModel)
		{
			var respuesta = new Response<Proyecto>();

			var planTrabajo = await GetPlanTrabajo(new PostGetPlanTrabajo { iIdPlanTrabajo = PostModel.iIdPlanTrabajo });

			if (planTrabajo.IsSuccess)
			{
				var jsonPlan = JsonSerializer.Serialize(planTrabajo.Data, new JsonSerializerOptions
				{
					WriteIndented = true,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				});


				var resquest = new RequestOpenAI
				{
					Prompt = GeneraPrompts.CreateFasesPrompt(jsonPlan),
					Configuration = GeneraPrompts.GenericPromptSystem(),
				};

				var llmresponse = await _azureIAService.CallAzureOpenIAJson(resquest);
				var jsonResponse = llmresponse.Content[0].Text;

				var tmpproyecto = JsonSerializer.Deserialize<Root>(jsonResponse);
				var proyecto = tmpproyecto.proyectos.SingleOrDefault();

				proyecto.iIdProyecto = 0;
				proyecto.iIdPlanTrabajo = PostModel.iIdPlanTrabajo;

				var postmodel = _mapperHelper.Get<Proyecto, PostSaveProyecto>(proyecto);

				var saveProyecto = await SaveProyecto(postmodel);


				if (saveProyecto.IsSuccess)
				{
					saveProyecto.Data.fases = new List<Fases>();
					foreach (var fase in proyecto.fases)
					{
						var postmodelFase = _mapperHelper.Get<Fases, PostSaveFase>(fase);
						postmodelFase.iIdFase = 0;
						postmodelFase.iIdProyecto = saveProyecto.Data.iIdProyecto;
						var saveFase = await SaveFase(postmodelFase);
						if (saveFase.IsSuccess)
						{
							fase.iIdFase = saveFase.Data.iIdFase;
							fase.iIdProyecto = saveFase.Data.iIdProyecto;
							saveProyecto.Data.fases.Add(fase);
						}


					}
				}


				respuesta.Data = saveProyecto.Data;
				respuesta.IsSuccess = true;

			}
			return respuesta;

		}

		public async Task<Response<IEnumerable<Proyecto>>> GetProyecto(PostGetProyecto PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostGetProyecto, spGetProyecto.Request>(PostModel);
				var result = await _planesRepository.spGetProyecto(model);
				var validation = _responseHelper.Validacion<spGetProyecto.Result, Proyecto>(result);
				if (validation.IsSuccess)
				{
					validation.Data = validation.Data;


					foreach (var proyecto in validation.Data)
					{
						var requestFase = new spGetFase.Request
						{
							iIdProyecto = proyecto.iIdProyecto
						};


						var resultFase = await _planesRepository.spGetFase(requestFase);
						var validationFase = _responseHelper.Validacion<spGetFase.Result, Fases>(resultFase);
						if (validationFase.IsSuccess)
						{

							foreach (var fase in validationFase.Data)
							{

								var requestPaso = new spGetPaso.Request
								{
									iIdFase = fase.iIdFase
								};

								var resultpaso = await _planesRepository.spGetPaso(requestPaso);
								var validationPaso = _responseHelper.Validacion<spGetPaso.Result, Paso>(resultpaso);

								if (validationPaso.IsSuccess)
								{

									fase.pasos = validationPaso.Data.ToList();

								}
							}

							proyecto.fases = validationFase.Data.ToList();

						}


					}

				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<IEnumerable<Proyecto>>.BadResult(ex.Message, new List<Proyecto>());
			}
		}



		public async Task<Response<Proyecto>> SaveProyecto(PostSaveProyecto PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostSaveProyecto, spSaveProyecto.Request>(PostModel);
				var result = await _planesRepository.spSaveProyecto(model);
				var validation = _responseHelper.Validacion<spSaveProyecto.Result, Proyecto>(result);
				if (validation.IsSuccess)
				{
					PostModel.iIdProyecto = validation.Data.iIdProyecto;
					validation.Data = _mapperHelper.Get<PostSaveProyecto, Proyecto>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<Proyecto>.BadResult(ex.Message, new());
			}
		}

		public async Task<Response<Fases>> SaveFase(PostSaveFase PostModel)
		{
			try
			{

				var model = _mapperHelper.Get<PostSaveFase, spSaveFase.Request>(PostModel);

				var tmplist = new List<DTPaso>();

				foreach (var paso in PostModel.pasos)
				{
					var dtpaso = _mapperHelper.Get<Paso, DTPaso>(paso);

					tmplist.Add(dtpaso);
				}
				model.LstPasos = tmplist;

				var result = await _planesRepository.spSaveFase(model);
				var validation = _responseHelper.Validacion<spSaveFase.Result, Fases>(result);
				if (validation.IsSuccess)
				{
					PostModel.iIdFase = validation.Data.iIdFase;
					validation.Data = _mapperHelper.Get<PostSaveFase, Fases>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<Fases>.BadResult(ex.Message, new());
			}
		}




		// Método para sanitizar entradas
		private string SanitizeInput(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return input;

			// Eliminar caracteres especiales o palabras potencialmente sensibles
			// Este es un ejemplo básico; ajusta según necesidades específicas
			var sanitized = Regex.Replace(input, @"[^\w\s.,-]", "");
			// Opcional: Reemplazar palabras sensibles (puedes definir una lista)
			var sensitiveWords = new List<string> { "violencia", "ilegal", "explícito" }; // Ejemplo
			foreach (var word in sensitiveWords)
			{
				sanitized = sanitized.Replace(word, "[REDACTED]", StringComparison.OrdinalIgnoreCase);
			}
			return sanitized.Trim();
		}

		public async Task<Response<Modulo>> SaveModulo(PostSaveModulo PostModel)
		{
			try
			{
				var model = new spSaveModulo.Request
				{
					iIdModulo = PostModel.Modulo.iIdModulo,
					iIdPlanTrabajo = PostModel.Modulo.iIdPlanTrabajo,
					vchTitulo = PostModel.Modulo.vchTitulo,
					nvchProposito = PostModel.Modulo.nvchProposito,
				};
				var tmpList = new List<DTFuncionalidad>();

				foreach (var item in PostModel.Modulo.LstFuncionalidades)
				{
					var funcionalidad = _mapperHelper.Get<Funcionalidades, DTFuncionalidad>(item);

					tmpList.Add(funcionalidad);
				}

				model.LstFuncionalidades = tmpList;

				var result = await _planesRepository.spSaveModulo(model);
				var validation = _responseHelper.Validacion<spSaveModulo.Result, Modulo>(result);
				if (validation.IsSuccess)
				{
					PostModel.Modulo.iIdModulo = validation.Data.iIdModulo;
					validation.Data = _mapperHelper.Get<PostSaveModulo, Modulo>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<Modulo>.BadResult(ex.Message, new());
			}
		}





	}

	public class Root
	{
		public List<Proyecto> proyectos { get; set; }
	}
}

