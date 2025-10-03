using EngramaCoreStandar;

namespace WorkFlow.Share.Objetos.Planes
{
	public class Paso
	{
		public int iIdPaso { get; set; }
		public int iIdFase { get; set; }
		public int smNumeroSecuencia { get; set; }
		public string nvchDescripcion { get; set; }
		public string nvchProposito { get; set; }
		public string nvchCaracteristicas { get; set; }
		public string nvchEnfoque { get; set; }
		public DateTime? dtCreadoEn { get; set; }
		public DateTime? dtActualizadoEn { get; set; }


		public Paso()
		{
			nvchDescripcion = string.Empty;
			nvchProposito = string.Empty;
			nvchCaracteristicas = string.Empty;
			nvchEnfoque = string.Empty;
			dtCreadoEn = Defaults.SqlMinDate();
			dtActualizadoEn = Defaults.SqlMinDate();
		}
	}
}
