using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spGetProyecto
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spGetProyecto"; }
			public int iIdProyecto { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdProyecto { get; set; }
			public string nvchNombre { get; set; }
			public string nvchDescripcion { get; set; }
			public DateTime? dtCreadoEn { get; set; }
			public DateTime? dtActualizadoEn { get; set; }
			public int iIdPlanTrabajo { get; set; }
		}
	}

}
