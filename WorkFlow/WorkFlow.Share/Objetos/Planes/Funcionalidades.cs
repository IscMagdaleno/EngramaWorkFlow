namespace WorkFlow.Share.Objetos.Planes
{
	public class Funcionalidades
	{
		public int iIdFuncionalidad { get; set; }
		public int iIdPlanTrabajo { get; set; }
		public string vchNombre { get; set; }
		public string nvchDescripcion { get; set; }
		public string nvchProceso { get; set; }
		public string vchComponentes { get; set; }
		public string nvchDatosMovidos { get; set; }
		public string vchEstatus { get; set; }
		public Funcionalidades()
		{
			vchNombre = string.Empty;
			nvchDescripcion = string.Empty;
			nvchProceso = string.Empty;
			vchComponentes = string.Empty;
			nvchDatosMovidos = string.Empty;
			vchEstatus = string.Empty;
		}

	}
}
