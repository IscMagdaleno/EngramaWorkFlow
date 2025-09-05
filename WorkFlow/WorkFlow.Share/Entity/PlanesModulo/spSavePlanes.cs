using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.PlanesModulo
{
	public class spSavePlanes
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSavePlanes"; }
			public int iPlanID { get; set; }
			public int iUsuarioID { get; set; }
			public string nvchTitulo { get; set; }
			public string nvchDescripcion { get; set; }
			public string nvchEstado { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iPlanID { get; set; }
		}
	}

}
