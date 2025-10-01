namespace WorkFlow.Share.Entity.Planes
{
	public class DTFuncionalidad
	{
		public int iIdFuncionalidad { get; set; }
		public int iIdModulo { get; set; }
		public string nvchDescripcion { get; set; }
		public string nvchEntidades { get; set; }
		public string nvchInteracciones { get; set; }
		public string nvchTecnico { get; set; }
		public string nvchConsideraciones { get; set; }

		public DTFuncionalidad()
		{
			nvchDescripcion = string.Empty;
			nvchEntidades = string.Empty;
			nvchInteracciones = string.Empty;
			nvchTecnico = string.Empty;
			nvchConsideraciones = string.Empty;
		}
	}
}
