using Microsoft.AspNetCore.Mvc;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.Share.PostClass.Planes;

namespace WorkFlow.API.Controllers
{


	[ApiController]
	[Route("api/[controller]")]
	public class PlanesController : ControllerBase
	{
		private readonly IPlanesDominio _planesDominio;

		public PlanesController(IPlanesDominio planesDominio)
		{
			_planesDominio = planesDominio;
		}


		/// <summary>
		/// Guarda los planes de trabajo en la base de datos
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostSavePlanTrabajo")]
		public async Task<IActionResult> PostSavePlanTrabajo([FromBody] PostSavePlanTrabajo postModel)
		{
			var result = await _planesDominio.SavePlanTrabajo(postModel);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}




		/// <summary>
		/// Obtener los planes de trabajo de la base de datos
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostGetPlanTrabajo")]
		public async Task<IActionResult> PostGetPlanTrabajo([FromBody] PostGetPlanTrabajo postModel)
		{
			var result = await _planesDominio.GetPlanTrabajo(postModel);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}



		/// <summary>
		/// Genera las fases para el desarrollo de la aplicación
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostGeneraFasesDesarrollo")]
		public async Task<IActionResult> PostGeneraFasesDesarrollo([FromBody] PostGeneraFasesDesarrollo postModel)
		{
			var result = await _planesDominio.GeneraFasesDesarrollo(postModel);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}


		/// <summary>
		/// Se guarda el modulo con la lista de Funcionalidades
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostSaveModulo")]
		public async Task<IActionResult> PostSaveModulo([FromBody] PostSaveModulo postModel)
		{
			var result = await _planesDominio.SaveModulo(postModel);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

	}
}
