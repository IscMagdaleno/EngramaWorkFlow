using Microsoft.AspNetCore.Mvc;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.Share.PostClass.PlanesModulo;

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
		/// Guardar inicial del plan de trabajo
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostSavePlanes")]
		public async Task<IActionResult> PostSavePlanes([FromBody] PostSavePlanes postModel)
		{
			var result = await _planesDominio.SavePlanes(postModel);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		/// <summary>
		/// Guarda la respuesta a lasa preguntas iniciales del proyecto
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostSaveRespuestas")]
		public async Task<IActionResult> PostSaveRespuestas([FromBody] PostSaveRespuestas postModel)
		{
			var result = await _planesDominio.SaveRespuestas(postModel);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

	}
}
