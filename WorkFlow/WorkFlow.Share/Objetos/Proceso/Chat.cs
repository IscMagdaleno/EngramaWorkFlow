using EngramaCoreStandar;

namespace WorkFlow.Share.Objetos.Proceso
{
	public class Chat
	{
		public int iIdChat { get; set; }
		public int iIdFase { get; set; }
		public DateTime? dtFechaCreacion { get; set; }
		public string nvchNombre { get; set; }
		public bool bActivo { get; set; }

		public List<Mensaje> LstMensajes { get; set; }
		public Chat()
		{
			dtFechaCreacion = Defaults.SqlMinDate();
			nvchNombre = string.Empty;
			LstMensajes = new List<Mensaje>();
		}

	}
}
