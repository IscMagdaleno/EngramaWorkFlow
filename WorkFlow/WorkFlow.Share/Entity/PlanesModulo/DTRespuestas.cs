namespace WorkFlow.Share.Entity.PlanesModulo
{
	public class DTRespuestas
	{
		public int iRespuestaID { get; set; }
		public int iPlanID { get; set; }
		public string nvchPregunta { get; set; }
		public string nvchRespuesta { get; set; }
		public DateTime? dtFechaCreacion { get; set; }
	}
}
