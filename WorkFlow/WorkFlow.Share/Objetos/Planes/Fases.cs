
using EngramaCoreStandar;

namespace WorkFlow.Share.Objetos.Planes
{
	public class Fases
	{
		public int iIdFase { get; set; }
		public int iIdProyecto { get; set; }
		public int smNumeroSecuencia { get; set; }
		public string nvchTitulo { get; set; }
		public string nvchDescripcion { get; set; }
		public DateTime? dtCreadoEn { get; set; }
		public DateTime? dtActualizadoEn { get; set; }
		public IList<Paso> pasos { get; set; }

		public Fases()
		{
			nvchTitulo = string.Empty;
			nvchDescripcion = string.Empty;
			dtCreadoEn = Defaults.SqlMinDate();
			dtActualizadoEn = Defaults.SqlMinDate();
			pasos = new List<Paso>();
		}
	}
}

