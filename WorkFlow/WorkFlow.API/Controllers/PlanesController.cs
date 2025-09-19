using Microsoft.AspNetCore.Mvc;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.Share.PostClass.Planes;

namespace WorkFlow.API.Controllers
{

	/// <summary>
	/// Test controller to show how Engrama work
	/// </summary>
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
		/// Guarda las funcionalidades a desarrollar en el proyecto
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostSaveFuncionalidades")]
		public async Task<IActionResult> PostSaveFuncionalidades([FromBody] PostSaveFuncionalidades postModel)
		{
			var result = await _planesDominio.SaveFuncionalidades(postModel);
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


	}
}
