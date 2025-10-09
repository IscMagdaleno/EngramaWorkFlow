using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Proceso
{
	public class spGetChat
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spGetChat"; }
			public int iIdFase { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdChat { get; set; }
			public int iIdFase { get; set; }
			public DateTime? dtFechaCreacion { get; set; }
			public string nvchNombre { get; set; }
			public bool bActivo { get; set; }
		}
	}

}
