using WorkFlow.Share.Entity.PlanesModulo;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Interfaces
{
	public interface IPlanesRepository
	{
		Task<spGetRespuestasByPlan.Result> spGetRespuestasByPlan(spGetRespuestasByPlan.Request PostModel);
		Task<spSaveFases.Result> spSaveFases(spSaveFases.Request PostModel);
		Task<spSavePlanes.Result> spSavePlanes(spSavePlanes.Request PostModel);
		Task<spSaveRespuestas.Result> spSaveRespuestas(spSaveRespuestas.Request PostModel);
	}
}
