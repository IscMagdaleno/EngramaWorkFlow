using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Proceso
{
	public class spSaveChat
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSaveChat"; }
			public int iIdChat { get; set; }
			public int iIdFuncionalidad { get; set; }
			public DateTime? dtFechaCreacion { get; set; }
			public string nvchNombre { get; set; }
			public bool bActivo { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdChat { get; set; }
		}
	}

}
