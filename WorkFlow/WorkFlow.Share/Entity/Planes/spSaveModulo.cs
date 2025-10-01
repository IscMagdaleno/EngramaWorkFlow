using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spSaveModulo
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSaveModulo"; }
			public int iIdModulo { get; set; }
			public int iIdPlanTrabajo { get; set; }
			public string vchTitulo { get; set; }
			public string nvchProposito { get; set; }
			public IEnumerable<DTFuncionalidad> LstFuncionalidades { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdModulo { get; set; }
		}
	}

}
