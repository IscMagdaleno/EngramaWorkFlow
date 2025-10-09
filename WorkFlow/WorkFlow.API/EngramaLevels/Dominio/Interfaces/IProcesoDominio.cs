using EngramaCoreStandar.Results;

using WorkFlow.Share.Objetos.Proceso;
using WorkFlow.Share.PostClass.Proceso;

namespace WorkFlow.API.EngramaLevels.Dominio.Interfaces
{
	public interface IProcesoDominio
	{
		Task<Response<Mensaje>> GetConversation(PostConversacion PostModel);
	}
}
