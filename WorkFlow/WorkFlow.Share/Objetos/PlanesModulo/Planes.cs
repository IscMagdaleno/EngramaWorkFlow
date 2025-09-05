using EngramaCoreStandar;

namespace WorkFlow.Share.Objetos.PlanesModulo
{
	public class Planes
	{
		public int iPlanID { get; set; }
		public int iUsuarioID { get; set; }
		public string nvchTitulo { get; set; }
		public string nvchDescripcion { get; set; }
		public DateTime? dtFechaCreacion { get; set; }
		public string nvchEstado { get; set; }
		public Planes()
		{
			nvchTitulo = string.Empty;
			nvchDescripcion = string.Empty;
			dtFechaCreacion = Defaults.SqlMinDate();
			nvchEstado = string.Empty;
		}

	}
}
