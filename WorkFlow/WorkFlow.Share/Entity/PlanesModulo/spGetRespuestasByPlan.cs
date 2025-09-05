using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.PlanesModulo
{
	public class spGetRespuestasByPlan
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spGetRespuestasByPlan"; }
			public int iPlanID { get; set; }
		}

		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public string nvchTitulo { get; set; }
			public string RespuestasJson { get; set; }  // Lista parseada desde JSON en el dominio
		}
	}

}
