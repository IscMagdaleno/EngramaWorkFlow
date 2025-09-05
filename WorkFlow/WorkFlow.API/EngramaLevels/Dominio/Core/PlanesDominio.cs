using EngramaCoreStandar.Dapper.Results;
using EngramaCoreStandar.Mapper;
using EngramaCoreStandar.Results;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.Share.Entity.PlanesModulo;
using WorkFlow.Share.Objetos.PlanesModulo;
using WorkFlow.Share.PostClass.PlanesModulo;

namespace WorkFlow.API.EngramaLevels.Dominio.Core
{
	public class PlanesDominio : IPlanesDominio
	{
		private readonly MapperHelper mapperHelper;
		private readonly IResponseHelper responseHelper;
		private readonly IPlanesRepository _planesRepository;

		/// <summary>
		/// Initialize the fields receiving the interfaces on the builder
		/// </summary>
		public PlanesDominio(
			MapperHelper mapperHelper,
			IResponseHelper responseHelper,
			IPlanesRepository planesRepository)
		{
			this.mapperHelper = mapperHelper;
			this.responseHelper = responseHelper;
			_planesRepository = planesRepository;
		}


		public async Task<Response<Planes>> SavePlanes(PostSavePlanes PostModel)
		{
			try
			{
				var model = mapperHelper.Get<PostSavePlanes, spSavePlanes.Request>(PostModel);
				var result = await _planesRepository.spSavePlanes(model);
				var validation = responseHelper.Validacion<spSavePlanes.Result, Planes>(result);
				if (validation.IsSuccess)
				{
					PostModel.iPlanID = validation.Data.iPlanID;
					validation.Data = mapperHelper.Get<PostSavePlanes, Planes>(PostModel);
				}
				return validation;
			}
			catch (Exception ex)
			{
				return Response<Planes>.BadResult(ex.Message, new());
			}
		}

		public async Task<Response<GenericResponse>> SaveRespuestas(PostSaveRespuestas PostModel)
		{
			try
			{

				List<DTRespuestas> RespuestasList = new List<DTRespuestas>();

				foreach (var item in PostModel.RespuestasList)
				{
					RespuestasList.Add(new DTRespuestas
					{
						dtFechaCreacion = item.dtFechaCreacion,
						iPlanID = item.iPlanID,
						iRespuestaID = item.iRespuestaID,
						nvchPregunta = item.nvchPregunta,
						nvchRespuesta = item.nvchRespuesta,

					});
				}

				var model = new spSaveRespuestas.Request
				{
					RespuestasList = RespuestasList
				};

				var result = await _planesRepository.spSaveRespuestas(model);
				var validation = responseHelper.Validacion<spSaveRespuestas.Result, GenericResponse>(result);

				return validation;
			}
			catch (Exception ex)
			{
				return Response<GenericResponse>.BadResult(ex.Message, new());
			}
		}


	}
}
