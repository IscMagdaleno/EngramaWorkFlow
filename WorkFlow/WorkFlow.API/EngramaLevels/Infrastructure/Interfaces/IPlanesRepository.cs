using WorkFlow.Share.Entity.Planes;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Interfaces
{
	public interface IPlanesRepository
	{
		Task<IEnumerable<spGetFuncionalidad.Result>> spGetFuncionalidad(spGetFuncionalidad.Request PostModel);
		Task<IEnumerable<spGetModulo.Result>> spGetModulo(spGetModulo.Request PostModel);
		Task<IEnumerable<spGetPlanTrabajo.Result>> spGetPlanTrabajo(spGetPlanTrabajo.Request PostModel);
		Task<spSaveModulo.Result> spSaveModulo(spSaveModulo.Request PostModel);
		Task<spSavePlanTrabajo.Result> spSavePlanTrabajo(spSavePlanTrabajo.Request PostModel);
	}
}
