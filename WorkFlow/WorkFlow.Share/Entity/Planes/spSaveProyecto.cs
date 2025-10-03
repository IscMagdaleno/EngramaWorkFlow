using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spSaveProyecto
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSaveProyecto"; }
			public int iIdProyecto { get; set; }
			public string nvchNombre { get; set; }
			public string nvchDescripcion { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdProyecto { get; set; }
		}
	}

}
