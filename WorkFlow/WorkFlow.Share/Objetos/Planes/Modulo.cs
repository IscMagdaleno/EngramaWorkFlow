namespace WorkFlow.Share.Objetos.Planes
{
	public class Modulo
	{
		public int IdModulo { get; set; }
		public int IdPlanTrabajo { get; set; }
		public string vchTitulo { get; set; }
		public string nvchProposito { get; set; }

		public List<Funcionalidades> LstFuncionalidades { get; set; }

		public Modulo()
		{
			vchTitulo = string.Empty;
			nvchProposito = string.Empty;
			LstFuncionalidades = new List<Funcionalidades>();
		}
	}

}
