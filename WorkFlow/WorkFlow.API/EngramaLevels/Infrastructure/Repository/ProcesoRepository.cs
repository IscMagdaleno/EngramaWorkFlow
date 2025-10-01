using EngramaCoreStandar.Dapper;
using EngramaCoreStandar.Extensions;

using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.Proceso;

namespace WorkFlow.API.EngramaLevels.Infrastructure.Repository
{
	public class ProcesoRepository : IProcesoRepository
	{

		private readonly IDapperManagerHelper _managerHelper;

		/// <summary>
		/// constructor to initialize all the class and receive the other interfaces how we will to work
		/// </summary>
		/// <param name="managerHelper"></param>
		public ProcesoRepository(IDapperManagerHelper managerHelper)
		{
			_managerHelper = managerHelper;
		}


		public async Task<spSaveChat.Result> spSaveChat(spSaveChat.Request PostModel)
		{
			var result = await _managerHelper.GetAsync<spSaveChat.Result, spSaveChat.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}


		public async Task<spGetChat.Result> spGetChat(spGetChat.Request PostModel)
		{
			var result = await _managerHelper.GetAsync<spGetChat.Result, spGetChat.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}


		public async Task<IEnumerable<spGetMensaje.Result>> spGetMensaje(spGetMensaje.Request PostModel)
		{
			var result = await _managerHelper.GetAllAsync<spGetMensaje.Result, spGetMensaje.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new List<spGetMensaje.Result>() { new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" } };
		}

		public async Task<spSaveMensaje.Result> spSaveMensaje(spSaveMensaje.Request PostModel)
		{
			var result = await _managerHelper.GetAsync<spSaveMensaje.Result, spSaveMensaje.Request>(PostModel);
			if (result.Ok)
			{
				return result.Data;
			}
			return new() { bResult = false, vchMessage = $"[{(result.Ex.NotNull() ? result.Ex.Message : "")}] - [{result.Msg}]" };
		}

	}
}
