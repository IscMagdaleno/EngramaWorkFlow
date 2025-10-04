using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spGetPaso
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spGetPaso"; }
			public int iIdFase { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdPaso { get; set; }
			public int iIdFase { get; set; }
			public int smNumeroSecuencia { get; set; }
			public string nvchDescripcion { get; set; }
			public string nvchProposito { get; set; }
			public string nvchCaracteristicas { get; set; }
			public string nvchEnfoque { get; set; }
			public DateTime? dtCreadoEn { get; set; }
			public DateTime? dtActualizadoEn { get; set; }
		}
	}

}
