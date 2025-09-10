using EngramaCoreStandar.Dapper;

using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;

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


	}
}
