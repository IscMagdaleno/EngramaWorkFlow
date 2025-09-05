using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.PlanesModulo
{
	public class spSaveRespuestas
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSaveRespuestas"; }
			public IEnumerable<DTRespuestas> RespuestasList { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iInsertedCount { get; set; }
		}
	}

}
