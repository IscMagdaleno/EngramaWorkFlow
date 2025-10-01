using WorkFlow.Share.Entity.Proceso;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Interfaces
{
	public interface IProcesoRepository
	{
		Task<spGetChat.Result> spGetChat(spGetChat.Request PostModel);
		Task<IEnumerable<spGetMensaje.Result>> spGetMensaje(spGetMensaje.Request PostModel);
		Task<spSaveChat.Result> spSaveChat(spSaveChat.Request PostModel);
		Task<spSaveMensaje.Result> spSaveMensaje(spSaveMensaje.Request PostModel);
	}
}
