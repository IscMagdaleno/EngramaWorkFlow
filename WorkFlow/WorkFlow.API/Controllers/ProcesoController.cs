using Microsoft.AspNetCore.Mvc;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.Share.PostClass.Proceso;

namespace WorkFlow.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProcesoController : ControllerBase
	{

		private readonly IProcesoDominio _procesoDominio;

		public ProcesoController(IProcesoDominio procesoDominio)
		{
			_procesoDominio = procesoDominio;
		}


		/// <summary>
		/// Guarda los planes de trabajo en la base de datos
		/// </summary>
		/// <param name="postModel"></param>
		/// <returns></returns>
		[HttpPost("PostConversacion")]
		public async Task<IActionResult> PostConversacion([FromBody] PostConversacion postModel)
		{
			var result = await _procesoDominio.GetConversation(postModel);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}
	}
}
