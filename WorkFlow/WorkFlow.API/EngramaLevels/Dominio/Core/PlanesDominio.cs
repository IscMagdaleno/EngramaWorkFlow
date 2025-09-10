using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;

namespace WorkFlow.API.EngramaLevels.Dominio.Core
{
	public class PlanesDominio : IPlanesDominio
	{
		private readonly MapperHelper _mapperHelper;
		private readonly IResponseHelper _responseHelper;
		private readonly IPlanesRepository _planesRepository;
		private readonly IAzureIAService _azureIAService;
		/// <summary>
		/// Initialize the fields receiving the interfaces on the builder
		/// </summary>
		public PlanesDominio(
			MapperHelper mapperHelper,
			IResponseHelper responseHelper,
			IPlanesRepository planesRepository,
			IAzureIAService azureIAService)
		{
			_mapperHelper = mapperHelper;
			_responseHelper = responseHelper;
			_planesRepository = planesRepository;
			_azureIAService = azureIAService;
		}




	}
}

