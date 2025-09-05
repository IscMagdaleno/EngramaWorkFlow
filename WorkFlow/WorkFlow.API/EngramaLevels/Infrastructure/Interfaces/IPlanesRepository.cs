using WorkFlow.Share.Entity.PlanesModulo;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Interfaces
{
	public interface IPlanesRepository
	{
		Task<spSavePlanes.Result> spSavePlanes(spSavePlanes.Request PostModel);
		Task<spSaveRespuestas.Result> spSaveRespuestas(spSaveRespuestas.Request PostModel);
	}
}
