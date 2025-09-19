using WorkFlow.Share.Entity.Planes;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Interfaces
{
	public interface IPlanesRepository
	{
		Task<IEnumerable<spGetFuncionalidades.Result>> spGetFuncionalidades(spGetFuncionalidades.Request PostModel);
		Task<IEnumerable<spGetPlanTrabajo.Result>> spGetPlanTrabajo(spGetPlanTrabajo.Request PostModel);
		Task<spSaveFuncionalidades.Result> spSaveFuncionalidades(spSaveFuncionalidades.Request PostModel);
		Task<spSavePlanTrabajo.Result> spSavePlanTrabajo(spSavePlanTrabajo.Request PostModel);
	}
}
