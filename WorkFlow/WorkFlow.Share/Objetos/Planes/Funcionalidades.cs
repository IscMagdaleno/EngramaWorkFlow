namespace WorkFlow.Share.Objetos.Planes
{
	public class Funcionalidades
	{
		public int iIdFuncionalidad { get; set; }
		public int iIdModulo { get; set; }
		public string nvchDescripcion { get; set; }
		public List<string> nvchEntidades { get; set; }
		public string nvchInteracciones { get; set; }
		public string nvchTecnico { get; set; }
		public string nvchConsideraciones { get; set; }
		public Funcionalidades()
		{
			nvchDescripcion = string.Empty;
			nvchEntidades = new List<string>();
			nvchInteracciones = string.Empty;
			nvchTecnico = string.Empty;
			nvchConsideraciones = string.Empty;
		}

	}
}
