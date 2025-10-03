namespace WorkFlow.Share.Objetos.Planes
{
	public class PlanTrabajo
	{
		public int iIdPlanTrabajo { get; set; }
		public string vchNombre { get; set; }
		public string nvchDescripcion { get; set; }
		public string vchEstatus { get; set; }

		public List<Modulo> LstModulos { get; set; }


		public PlanTrabajo()
		{
			vchNombre = string.Empty;
			nvchDescripcion = string.Empty;
			vchEstatus = string.Empty;

			LstModulos = new List<Modulo>();
		}

	}
}
