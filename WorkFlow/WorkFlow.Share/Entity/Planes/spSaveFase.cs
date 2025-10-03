using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spSaveFase
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSaveFase"; }
			public int iIdFase { get; set; }
			public int iIdProyecto { get; set; }
			public int smNumeroSecuencia { get; set; }
			public string nvchTitulo { get; set; }
			public string nvchDescripcion { get; set; }
			public DateTime? dtCreadoEn { get; set; }
			public DateTime? dtActualizadoEn { get; set; }
			public List<DTPaso> LstPasos { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdFase { get; set; }
		}
	}

}
