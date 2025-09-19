using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.Planes;
using WorkFlow.Share.Objetos.Planes;
using WorkFlow.Share.PostClass.Planes;

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



		public async Task<Response<PlanTrabajo>> SavePlanTrabajo(PostSavePlanTrabajo PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostSavePlanTrabajo, spSavePlanTrabajo.Request>(PostModel);
				var result = await _planesRepository.spSavePlanTrabajo(model);
				var validation = _responseHelper.Validacion<spSavePlanTrabajo.Result, PlanTrabajo>(result);
				if (validation.IsSuccess)
				{
					PostModel.iIdPlanTrabajo = validation.Data.iIdPlanTrabajo;
					validation.Data = _mapperHelper.Get<PostSavePlanTrabajo, PlanTrabajo>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<PlanTrabajo>.BadResult(ex.Message, new());
			}
		}
		public async Task<Response<Funcionalidades>> SaveFuncionalidades(PostSaveFuncionalidades PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostSaveFuncionalidades, spSaveFuncionalidades.Request>(PostModel);
				var result = await _planesRepository.spSaveFuncionalidades(model);
				var validation = _responseHelper.Validacion<spSaveFuncionalidades.Result, Funcionalidades>(result);
				if (validation.IsSuccess)
				{
					PostModel.iIdFuncionalidad = validation.Data.iIdFuncionalidad;
					validation.Data = _mapperHelper.Get<PostSaveFuncionalidades, Funcionalidades>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<Funcionalidades>.BadResult(ex.Message, new());
			}
		}

		public async Task<Response<IEnumerable<PlanTrabajo>>> GetPlanTrabajo(PostGetPlanTrabajo PostModel)
		{
			try
			{
				var model = _mapperHelper.Get<PostGetPlanTrabajo, spGetPlanTrabajo.Request>(PostModel);
				var result = await _planesRepository.spGetPlanTrabajo(model);
				var validation = _responseHelper.Validacion<spGetPlanTrabajo.Result, PlanTrabajo>(result);
				if (validation.IsSuccess)
				{
					foreach (var item in validation.Data)
					{

						var funcionalidadModel = new spGetFuncionalidades.Request
						{
							iIdPlanTrabajo = item.iIdPlanTrabajo
						};

						var resultFuncionalidades = await _planesRepository.spGetFuncionalidades(funcionalidadModel);
						var validationfuncialidades = _responseHelper.Validacion<spGetFuncionalidades.Result, Funcionalidades>(resultFuncionalidades);

						if (validationfuncialidades.IsSuccess)
						{
							item.LstFuncionalidadess = validationfuncialidades.Data.ToList();
						}

					}

					validation.Data = validation.Data;
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<IEnumerable<PlanTrabajo>>.BadResult(ex.Message, new List<PlanTrabajo>());
			}
		}



	}
}

