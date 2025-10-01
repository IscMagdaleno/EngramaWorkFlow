﻿using EngramaCoreStandar.Dapper.Interfaces;

namespace WorkFlow.Share.Entity.Proceso
{
	public class spGetMensaje
	{
		public class Request : SpRequest
		{
			public string StoredProcedure { get => "spGetMensaje"; }
			public int iIdChat { get; set; }
		}
		public class Result : DbResult
		{
			public bool bResult { get; set; }
			public string vchMessage { get; set; }
			public int iIdMensaje { get; set; }
			public int iIdChat { get; set; }
			public int iOrden { get; set; }
			public string nvchRol { get; set; }
			public string nvchContenido { get; set; }
			public DateTime? dtFecha { get; set; }
		}
	}

}
