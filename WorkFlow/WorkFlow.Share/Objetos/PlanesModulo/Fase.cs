using EngramaCoreStandar;

namespace WorkFlow.Share.Objetos.PlanesModulo
{
	public class Fase
	{
		public int iFaseID { get; set; }
		public int iPlanID { get; set; }
		public int iNumeroFase { get; set; }
		public string nvchTitulo { get; set; }
		public string nvchDescripcion { get; set; }
		public string nvchEstado { get; set; }
		public DateTime? dtFechaCreacion { get; set; }

		public Fase()
		{
			nvchTitulo = string.Empty;
			nvchDescripcion = string.Empty;
			nvchEstado = "Pendiente";
			dtFechaCreacion = Defaults.SqlMinDate();
		}
	}
}
