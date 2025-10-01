using EngramaCoreStandar.Results;

using WorkFlow.Share.Objetos.Planes;
using WorkFlow.Share.PostClass.Planes;

namespace WorkFlow.API.EngramaLevels.Dominio.Interfaces
{
	public interface IPlanesDominio
	{
		Task<Response<IEnumerable<PlanTrabajo>>> GetPlanTrabajo(PostGetPlanTrabajo PostModel);
		Task<Response<Modulo>> SaveModulo(PostSaveModulo PostModel);
		Task<Response<PlanTrabajo>> SavePlanTrabajo(PostSavePlanTrabajo PostModel);
	}
}
