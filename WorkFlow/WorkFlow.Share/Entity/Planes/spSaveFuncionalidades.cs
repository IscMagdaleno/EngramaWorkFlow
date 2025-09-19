using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Planes
{
	public class spSaveFuncionalidades
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spSaveFuncionalidades"; }
			public int iIdFuncionalidad { get; set; }
			public int iIdPlanTrabajo { get; set; }
			public string vchNombre { get; set; }
			public string nvchDescripcion { get; set; }
			public string nvchProceso { get; set; }
			public string vchComponentes { get; set; }
			public string nvchDatosMovidos { get; set; }
			public string vchEstatus { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdFuncionalidad { get; set; }
		}
	}

}
