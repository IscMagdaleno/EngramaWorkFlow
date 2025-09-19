namespace WorkFlow.Share.Objetos.Planes
{
	public class PlanTrabajo
	{
		public int iIdPlanTrabajo { get; set; }
		public string vchNombre { get; set; }
		public string nvchDescripcion { get; set; }
		public string vchEstatus { get; set; }

		public List<Funcionalidades> LstFuncionalidadess { get; set; }
		public PlanTrabajo()
		{
			vchNombre = string.Empty;
			nvchDescripcion = string.Empty;
			vchEstatus = string.Empty;

			LstFuncionalidadess = new List<Funcionalidades>();
		}

	}
}
