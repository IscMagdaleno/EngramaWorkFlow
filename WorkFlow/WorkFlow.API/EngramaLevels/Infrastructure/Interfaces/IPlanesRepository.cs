using WorkFlow.Share.Entity.Planes;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Interfaces
{
	public interface IPlanesRepository
	{
		Task<IEnumerable<spGetFase.Result>> spGetFase(spGetFase.Request PostModel);
		Task<IEnumerable<spGetFuncionalidad.Result>> spGetFuncionalidad(spGetFuncionalidad.Request PostModel);
		Task<IEnumerable<spGetModulo.Result>> spGetModulo(spGetModulo.Request PostModel);
		Task<IEnumerable<spGetPaso.Result>> spGetPaso(spGetPaso.Request PostModel);
		Task<IEnumerable<spGetPlanTrabajo.Result>> spGetPlanTrabajo(spGetPlanTrabajo.Request PostModel);
		Task<IEnumerable<spGetProyecto.Result>> spGetProyecto(spGetProyecto.Request PostModel);
		Task<spSaveFase.Result> spSaveFase(spSaveFase.Request PostModel);
		Task<spSaveModulo.Result> spSaveModulo(spSaveModulo.Request PostModel);
		Task<spSavePlanTrabajo.Result> spSavePlanTrabajo(spSavePlanTrabajo.Request PostModel);
		Task<spSaveProyecto.Result> spSaveProyecto(spSaveProyecto.Request PostModel);
	}
}
