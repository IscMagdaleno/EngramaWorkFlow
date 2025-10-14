using EngramaCoreStandar.Extensions;
using EngramaCoreStandar.Results;

using OpenAI.Chat;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios.Utiles;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.Proceso;
using WorkFlow.Share.Objetos.Proceso;
using WorkFlow.Share.PostClass.Planes;
using WorkFlow.Share.PostClass.Proceso;

namespace WorkFlow.API.EngramaLevels.Dominio.Servicios
{
	public class ChatMemoryService
	{
		private readonly IProcesoRepository _procesoRepository;
		private readonly IResponseHelper _responseHelper;
		private readonly IPlanesDominio _planesDominio;

		public ChatMemoryService(IProcesoRepository procesoRepository,
		IResponseHelper responseHelper,
		IPlanesDominio planesDominio)
		{
			_procesoRepository = procesoRepository;
			_responseHelper = responseHelper;
			_planesDominio = planesDominio;
		}



		public async Task<Response<Chat>> GetChatPorFase(PostConversacion PostModel)
		{

			var data = new Response<Chat>();
			var iIdFase = PostModel.iIdFase;
			try
			{
				var chat = new Chat();

				var chatResponse = await _procesoRepository.spGetChat(new spGetChat.Request { iIdFase = iIdFase });
				var validationchatResponse = _responseHelper.Validacion<spGetChat.Result, Chat>(chatResponse);

				if (validationchatResponse.IsSuccess)
				{
					chat = validationchatResponse.Data;



				}
				else
				{
					var saveChatModel = new spSaveChat.Request
					{
						iIdFase = iIdFase,
						nvchNombre = $"Chat para Fase {iIdFase}",
						dtFechaCreacion = DateTime.Now,
					};

					var createResponse = await _procesoRepository.spSaveChat(saveChatModel);
					var validationcreateResponse = _responseHelper.Validacion<spSaveChat.Result, Chat>(createResponse);
					if (validationcreateResponse.IsSuccess)
					{
						validationchatResponse.Data.iIdChat = validationcreateResponse.Data.iIdChat;
						chat.iIdChat = validationcreateResponse.Data.iIdChat;
						chat.iIdFase = saveChatModel.iIdFase;
						chat.nvchNombre = saveChatModel.nvchNombre; ;
						chat.dtFechaCreacion = saveChatModel.dtFechaCreacion;


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

					var resultProyecto = await _planesDominio.GetProyecto(new PostGetProyecto { iIdProyecto = PostModel.iIdProyecto });
					var resultPlan = await _planesDominio.GetPlanTrabajo(new PostGetPlanTrabajo { iIdPlanTrabajo = PostModel.iIdPlanTrabajo });


					if (resultProyecto.IsSuccess && resultPlan.IsSuccess)
					{

						var proyect = resultProyecto.Data.SingleOrDefault();
						var plan = resultPlan.Data.SingleOrDefault();



						var systemPrompt = GeneraPrompts.FuncialidadPrompt(PostModel, proyect, plan);


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

				}





				var modelSaveMessajeUser = new spSaveMensaje.Request
				{
					iIdChat = chat.iIdChat,
					dtFecha = DateTime.Now,
					iIdMensaje = -1,
					iOrden = chat.LstMensajes.Count + 1,
					nvchContenido = PostModel.nvchContenido,
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

		public async Task<Response<Mensaje>> GuardarRespuestaLLM(ChatCompletion chatCompletion, Chat chat)
		{
			var response = new Response<Mensaje>();
			response.IsSuccess = false;
			response.Data = new Mensaje();
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
					var mensage = new Mensaje
					{
						iIdChat = modelSaveMessaje.iIdChat,
						dtFecha = modelSaveMessaje.dtFecha,
						iIdMensaje = validationSaveMessaje.Data.iIdMensaje,
						iOrden = modelSaveMessaje.iOrden,
						nvchContenido = modelSaveMessaje.nvchContenido,
						nvchRol = modelSaveMessaje.nvchRol
					};

					response.IsSuccess = true;
					response.Data = mensage;
					;
					return response;

				}

			}

			return response;
		}

	}
}
