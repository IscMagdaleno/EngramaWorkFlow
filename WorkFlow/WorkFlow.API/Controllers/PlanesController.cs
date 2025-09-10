using Microsoft.AspNetCore.Mvc;

using WorkFlow.API.EngramaLevels.Dominio.Interfaces;

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


	}
}
