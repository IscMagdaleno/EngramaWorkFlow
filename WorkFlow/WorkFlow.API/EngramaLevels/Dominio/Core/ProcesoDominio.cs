
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
using WorkFlow.API.EngramaLevels.Dominio.Servicios.Utiles;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Objetos.Proceso;
using WorkFlow.Share.PostClass.Planes;
using WorkFlow.Share.PostClass.Proceso;

namespace WorkFlow.API.EngramaLevels.Dominio.Core
{
	public class ProcesoDominio : IProcesoDominio
	{

		private readonly MapperHelper _mapperHelper;
		private readonly IResponseHelper _responseHelper;
		private readonly IProcesoRepository _procesoRepository;
		private readonly IPlanesDominio _planesDominio;
		private readonly ChatMemoryService _chatMemoryService;
		private readonly IAzureIAService _azureIAService;

		/// <summary>
		/// Initialize the fields receiving the interfaces on the builder
		/// </summary>
		public ProcesoDominio(
			MapperHelper mapperHelper,
			IResponseHelper responseHelper,
			IProcesoRepository procesoRepository,
			IPlanesDominio planesDominio,
			ChatMemoryService chatMemoryService,
			IAzureIAService azureIAService)
		{
			_mapperHelper = mapperHelper;
			_responseHelper = responseHelper;
			_procesoRepository = procesoRepository;
			_planesDominio = planesDominio;
			_chatMemoryService = chatMemoryService;
			_azureIAService = azureIAService;
		}



		public async Task<Response<Mensaje>> GetConversation(PostConversacion PostModel)
		{
			try
			{
				var resultado = new Response<Mensaje>();

				var resultProyecto = await _planesDominio.GetProyecto(new PostGetProyecto { iIdProyecto = PostModel.iIdProyecto });
				var resultPlan = await _planesDominio.GetPlanTrabajo(new PostGetPlanTrabajo { iIdPlanTrabajo = PostModel.iIdPlanTrabajo });


				if (resultProyecto.IsSuccess && resultPlan.IsSuccess)
				{

					var proyect = resultProyecto.Data.SingleOrDefault();
					var plan = resultPlan.Data.SingleOrDefault();


					var systemPrompt = GeneraPrompts.FuncialidadPrompt(PostModel, proyect, plan);

					var ResponseChat = await _chatMemoryService.GetChatPorFase(PostModel.iIdFase, PostModel.nvchContenido, systemPrompt);
					if (ResponseChat.IsSuccess)
					{

						var respustaLLM = await _azureIAService.CallAzureOpenIAWithMemory(ResponseChat.Data);

						var resultSaveMessage = await _chatMemoryService.GuardarRespuestaLLM(respustaLLM, ResponseChat.Data);

						return resultSaveMessage;

					}

				}

				return resultado;
			}
			catch (Exception ex)
			{
				return Response<Mensaje>.BadResult(ex.Message, new());
			}
		}


	}
}
