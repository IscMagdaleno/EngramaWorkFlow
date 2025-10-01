using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spGetFuncionalidad
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spGetFuncionalidad"; }
			public int iIdModulo { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdFuncionalidad { get; set; }
			public int iIdModulo { get; set; }
			public string nvchDescripcion { get; set; }
			public string nvchEntidades { get; set; }
			public string nvchInteracciones { get; set; }
			public string nvchTecnico { get; set; }
			public string nvchConsideraciones { get; set; }
		}
	}

}
