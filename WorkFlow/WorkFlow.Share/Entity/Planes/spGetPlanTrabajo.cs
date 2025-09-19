using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spGetPlanTrabajo
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spGetPlanTrabajo"; }
			public int iIdPlanTrabajo { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdPlanTrabajo { get; set; }
			public string vchNombre { get; set; }
			public string nvchDescripcion { get; set; }
			public string vchEstatus { get; set; }
		}
	}

}
