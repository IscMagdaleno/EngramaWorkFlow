using EngramaCoreStandar.Dapper;
using EngramaCoreStandar.Extensions;

using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.PlanesModulo;

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

		public async Task<spSavePlanes.Result> spSavePlanes(spSavePlanes.Request PostModel)
		{
			var result = await _managerHelper.GetAsync<spSavePlanes.Result, spSavePlanes.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}


		public async Task<spSaveRespuestas.Result> spSaveRespuestas(spSaveRespuestas.Request PostModel)
		{
			var result = await _managerHelper.GetWithListAsync<spSaveRespuestas.Result, spSaveRespuestas.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}

	}
}
