using EngramaCoreStandar.Dapper;
using EngramaCoreStandar.Extensions;

using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.Planes;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Repository
{
	public class PlanesRepository : IPlanesRepository
	{

		private readonly IDapperManagerHelper _managerHelper;

		/// <summary>
		/// constructor to initialize all the class and receive the other interfaces how we will to work
		/// </summary>
		/// <param name="managerHelper"></param>
		public PlanesRepository(IDapperManagerHelper managerHelper)
		{
			_managerHelper = managerHelper;
		}

		public async Task<spSavePlanTrabajo.Result> spSavePlanTrabajo(spSavePlanTrabajo.Request PostModel)
		{
			var result = await _managerHelper.GetAsync<spSavePlanTrabajo.Result, spSavePlanTrabajo.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}


		public async Task<IEnumerable<spGetPlanTrabajo.Result>> spGetPlanTrabajo(spGetPlanTrabajo.Request PostModel)
		{
			var result = await _managerHelper.GetAllAsync<spGetPlanTrabajo.Result, spGetPlanTrabajo.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new List<spGetPlanTrabajo.Result>() { new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" } };
		}

		public async Task<spSaveModulo.Result> spSaveModulo(spSaveModulo.Request PostModel)
		{
			var result = await _managerHelper.GetWithListAsync<spSaveModulo.Result, spSaveModulo.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}

		public async Task<IEnumerable<spGetModulo.Result>> spGetModulo(spGetModulo.Request PostModel)
		{
			var result = await _managerHelper.GetAllAsync<spGetModulo.Result, spGetModulo.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new List<spGetModulo.Result>() { new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" } };
		}



		public async Task<IEnumerable<spGetFuncionalidad.Result>> spGetFuncionalidad(spGetFuncionalidad.Request PostModel)
		{
			var result = await _managerHelper.GetAllAsync<spGetFuncionalidad.Result, spGetFuncionalidad.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new List<spGetFuncionalidad.Result>() { new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" } };
		}

		public async Task<spSaveProyecto.Result> spSaveProyecto(spSaveProyecto.Request PostModel)
		{
			var result = await _managerHelper.GetAsync<spSaveProyecto.Result, spSaveProyecto.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}

		public async Task<spSaveFase.Result> spSaveFase(spSaveFase.Request PostModel)
		{
			var result = await _managerHelper.GetWithListAsync<spSaveFase.Result, spSaveFase.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}


	}
}
