namespace WorkFlow.Share.Objetos.Planes
{

	public class Proyecto
	{
		public int iIdProyecto { get; set; }
		public string nvchNombre { get; set; }
		public string nvchDescripcion { get; set; }
		public DateTime dtCreadoEn { get; set; }
		public DateTime dtActualizadoEn { get; set; }
		public IList<Fases> fases { get; set; }

		public Proyecto()
		{
			nvchNombre = string.Empty;
			nvchDescripcion = string.Empty;
		}


		public class Rootobject
		{
			public Proyecto[] proyectos { get; set; }
		}

	}
}
