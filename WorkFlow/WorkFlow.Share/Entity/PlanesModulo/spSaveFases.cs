using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.PlanesModulo
{
	public class spSaveFases
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSaveFases"; }
			public IEnumerable<DTFases> FasesList { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iInsertedCount { get; set; }
		}
	}

	public class DTFases
	{
		public int iFaseID { get; set; }
		public int iPlanID { get; set; }
		public int iNumeroFase { get; set; }
		public string nvchTitulo { get; set; }
		public string nvchDescripcion { get; set; }
		public string nvchEstado { get; set; }
		public DateTime? dtFechaCreacion { get; set; }
		public DateTime? dtFechaCompletada { get; set; }
	}
}