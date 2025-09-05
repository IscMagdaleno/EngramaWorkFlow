using EngramaCoreStandar.Dapper.Results;
using EngramaCoreStandar.Results;

using WorkFlow.Share.Objetos.PlanesModulo;
using WorkFlow.Share.PostClass.PlanesModulo;

namespace WorkFlow.API.EngramaLevels.Dominio.Interfaces
{
	public interface IPlanesDominio
	{
		Task<Response<List<Fase>>> GenerateAndSavePlanAsync(PostGeneratePlan PostModel);
		Task<Response<Planes>> SavePlanes(PostSavePlanes PostModel);
		Task<Response<GenericResponse>> SaveRespuestas(PostSaveRespuestas PostModel);
	}
}
