using EngramaCoreStandar.Extensions;
using EngramaCoreStandar.Results;

using OpenAI.Chat;

using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.Proceso;
using WorkFlow.Share.Objetos.Proceso;

namespace WorkFlow.API.EngramaLevels.Dominio.Servicios
{
	public class ChatMemoryService
	{
		private readonly IProcesoRepository _procesoRepository;
		private readonly IResponseHelper _responseHelper;

		public ChatMemoryService(IProcesoRepository procesoRepository, IResponseHelper responseHelper)
		{
			_procesoRepository = procesoRepository;
			_responseHelper = responseHelper;
		}

		public async Task<Response<Chat>> GetChatPorFuncionalidad(int iIdFuncionalidad, string nvchContenido, string systemPrompt)
		{

			var data = new Response<Chat>();

			try
			{
				var chat = new Chat();

				var chatResponse = await _procesoRepository.spGetChat(new spGetChat.Request { iIdFuncionalidad = iIdFuncionalidad });
				var validationchatResponse = _responseHelper.Validacion<spGetChat.Result, Chat>(chatResponse);

				if (validationchatResponse.IsSuccess)
				{
					chat = validationchatResponse.Data;



				}
				else
				{
					var saveChatModel = new spSaveChat.Request
					{
						iIdFuncionalidad = iIdFuncionalidad,
						nvchNombre = $"Chat para Funcionalidad {iIdFuncionalidad}",
						dtFechaCreacion = DateTime.Now,
					};

					var createResponse = await _procesoRepository.spSaveChat(saveChatModel);
					var validationcreateResponse = _responseHelper.Validacion<spSaveChat.Result, Chat>(createResponse);
					if (validationcreateResponse.IsSuccess)
					{
						validationchatResponse.Data.iIdChat = validationcreateResponse.Data.iIdChat;
						chat.iIdChat = validationcreateResponse.Data.iIdChat;
						chat.iIdFuncionalidad = iIdFuncionalidad;
						chat.nvchNombre = $"Chat para Funcionalidad {iIdFuncionalidad}";
						chat.dtFechaCreacion = DateTime.Now;


					}
				}


				var historialResponse = await _procesoRepository.spGetMensaje(new spGetMensaje.Request { iIdChat = chat.iIdChat });
				var validationHistorialResponse = _responseHelper.Validacion<spGetMensaje.Result, Mensaje>(historialResponse);
				if (validationHistorialResponse.IsSuccess)
				{
					chat.LstMensajes = validationHistorialResponse.Data.ToList();
				}
				else
				{

					var modelSaveMessaje = new spSaveMensaje.Request
					{
						iIdChat = chat.iIdChat,
						dtFecha = DateTime.Now,
						iIdMensaje = -1,
						iOrden = 1,
						nvchContenido = systemPrompt,
						nvchRol = "system"
					};
					var resiltSaveMessaje = await _procesoRepository.spSaveMensaje(modelSaveMessaje);
					var validationSaveMessaje = _responseHelper.Validacion<spSaveMensaje.Result, Mensaje>(resiltSaveMessaje);

					if (validationSaveMessaje.IsSuccess)
					{
						chat.LstMensajes.Add(new Mensaje
						{
							iIdChat = modelSaveMessaje.iIdChat,
							dtFecha = modelSaveMessaje.dtFecha,
							iIdMensaje = validationSaveMessaje.Data.iIdMensaje,
							iOrden = 1,
							nvchContenido = modelSaveMessaje.nvchContenido,
							nvchRol = modelSaveMessaje.nvchRol
						});

					}

				}





				var modelSaveMessajeUser = new spSaveMensaje.Request
				{
					iIdChat = chat.iIdChat,
					dtFecha = DateTime.Now,
					iIdMensaje = -1,
					iOrden = chat.LstMensajes.Count + 1,
					nvchContenido = nvchContenido,
					nvchRol = "user"
				};
				var resiltSaveMessajeUser = await _procesoRepository.spSaveMensaje(modelSaveMessajeUser);
				var validationSaveMessajeUser = _responseHelper.Validacion<spSaveMensaje.Result, Mensaje>(resiltSaveMessajeUser);
				if (validationSaveMessajeUser.IsSuccess)
				{


					chat.LstMensajes.Add(new Mensaje
					{
						iIdChat = modelSaveMessajeUser.iIdChat,
						dtFecha = modelSaveMessajeUser.dtFecha,
						iIdMensaje = validationSaveMessajeUser.Data.iIdMensaje,
						iOrden = modelSaveMessajeUser.iOrden,
						nvchContenido = modelSaveMessajeUser.nvchContenido,
						nvchRol = modelSaveMessajeUser.nvchRol
					});
				}



				data.IsSuccess = true;
				data.Message = "";
				data.Data = chat;

				return data;



			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);

			}


		}

		public async Task<Response<Chat>> GuardarRespuestaLLM(ChatCompletion chatCompletion, Chat chat)
		{
			var response = new Response<Chat>();
			response.IsSuccess = false;
			response.Data = chat;
			var respuestaLLM = chatCompletion.Content[0].Text;
			if (respuestaLLM.NotEmpty())
			{
				var modelSaveMessaje = new spSaveMensaje.Request
				{
					iIdChat = chat.iIdChat,
					dtFecha = DateTime.Now,
					iIdMensaje = -1,
					iOrden = chat.LstMensajes.Count + 1,
					nvchContenido = respuestaLLM,
					nvchRol = "assistant"
				};
				var resiltSaveMessaje = await _procesoRepository.spSaveMensaje(modelSaveMessaje);
				var validationSaveMessaje = _responseHelper.Validacion<spSaveMensaje.Result, Mensaje>(resiltSaveMessaje);

				if (validationSaveMessaje.IsSuccess)
				{
					chat.LstMensajes.Add(new Mensaje
					{
						iIdChat = modelSaveMessaje.iIdChat,
						dtFecha = modelSaveMessaje.dtFecha,
						iIdMensaje = validationSaveMessaje.Data.iIdMensaje,
						iOrden = modelSaveMessaje.iOrden,
						nvchContenido = modelSaveMessaje.nvchContenido,
						nvchRol = modelSaveMessaje.nvchRol
					});

					response.IsSuccess = true;
					response.Data = chat;
					return response;

				}

			}

			return response;
		}

	}
}
