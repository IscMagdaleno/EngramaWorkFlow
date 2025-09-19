using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using System.Text.RegularExpressions;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
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
		public async Task<Response<Funcionalidades>> SaveFuncionalidades(PostSaveFuncionalidades PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostSaveFuncionalidades, spSaveFuncionalidades.Request>(PostModel);
				var result = await _planesRepository.spSaveFuncionalidades(model);
				var validation = _responseHelper.Validacion<spSaveFuncionalidades.Result, Funcionalidades>(result);
				if (validation.IsSuccess)
				{
					PostModel.iIdFuncionalidad = validation.Data.iIdFuncionalidad;
					validation.Data = _mapperHelper.Get<PostSaveFuncionalidades, Funcionalidades>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<Funcionalidades>.BadResult(ex.Message, new());
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
					//foreach (var item in validation.Data)
					//{

					//	var funcionalidadModel = new spGetFuncionalidades.Request
					//	{
					//		iIdPlanTrabajo = item.iIdPlanTrabajo
					//	};

					//	var resultFuncionalidades = await _planesRepository.spGetFuncionalidades(funcionalidadModel);
					//	var validationfuncialidades = _responseHelper.Validacion<spGetFuncionalidades.Result, Funcionalidades>(resultFuncionalidades);

					//	if (validationfuncialidades.IsSuccess)
					//	{
					//		item.LstModulos = validationfuncialidades.Data.ToList();
					//	}

					//}

					validation.Data = validation.Data;
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

	}
}

