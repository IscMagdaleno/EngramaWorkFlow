
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Objetos.Proceso;
using WorkFlow.Share.PostClass.Proceso;

namespace WorkFlow.API.EngramaLevels.Dominio.Core
{
	public class ProcesoDominio : IProcesoDominio
	{

		private readonly MapperHelper _mapperHelper;
		private readonly IResponseHelper _responseHelper;
		private readonly IProcesoRepository _procesoRepository;
		private readonly ChatMemoryService _chatMemoryService;
		private readonly IAzureIAService _azureIAService;

		/// <summary>
		/// Initialize the fields receiving the interfaces on the builder
		/// </summary>
		public ProcesoDominio(
			MapperHelper mapperHelper,
			IResponseHelper responseHelper,
			IProcesoRepository procesoRepository,
			ChatMemoryService chatMemoryService,
			IAzureIAService azureIAService)
		{
			_mapperHelper = mapperHelper;
			_responseHelper = responseHelper;
			_procesoRepository = procesoRepository;
			_chatMemoryService = chatMemoryService;
			_azureIAService = azureIAService;
		}



		public async Task<Response<Chat>> GetConversation(PostConversacion PostModel)
		{
			try
			{
				var resultado = new Response<Chat>();


				var systemPrompt = @" Eres un arquitecto de software experto en aplicaciones web con .NET, Clean Architecture, SQL Server y Blazor/MudBlazor. 
				Tu tarea es desglosar aplicaciones en módulos funcionales según Domain-Driven Design (DDD) y principios SOLID.";


				var ResponseChat = await _chatMemoryService.GetChatPorFuncionalidad(PostModel.iIdFuncionalidad, PostModel.nvchContenido, systemPrompt);
				if (ResponseChat.IsSuccess)
				{

					var respustaLLM = await _azureIAService.CallAzureOpenIAWithMemory(ResponseChat.Data);

					var resultSaveMessage = await _chatMemoryService.GuardarRespuestaLLM(respustaLLM, ResponseChat.Data);

					return resultSaveMessage;

				}


				return resultado;
			}
			catch (Exception ex)
			{
				return Response<Chat>.BadResult(ex.Message, new());
			}
		}


	}
}
