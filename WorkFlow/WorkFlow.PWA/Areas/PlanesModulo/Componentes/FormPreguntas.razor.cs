using Microsoft.AspNetCore.Components;

using MudBlazor;

using WorkFlow.PWA.Areas.PlanesModulo.Utiles;
using WorkFlow.PWA.Shared.Common;

namespace WorkFlow.PWA.Areas.PlanesModulo.Componentes
{
	public partial class FormPreguntas : EngramaComponent
	{

		[Parameter] public MainPlanes Data { get; set; }
		[Parameter] public EventCallback OnRespuestasSaved { get; set; }





		private MudForm _form;
		private bool _isValid;
		private string _successMessage;
		private string _errorMessage;


		protected override void OnInitialized()
		{

			Data.LstRespuestas = new()
			{
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Descripción general", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Problema principal o necesidad", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Objetivos clave", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Usuarios principales y roles", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Expectativas de usuarios", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Procesos manuales a automatizar", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Integraciones externas", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Funcionalidades principales", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Interacciones o flujos clave", dtFechaCreacion = DateTime.Now },
				new() { iPlanID = Data.PlanesSelected.iPlanID, nvchPregunta = "Requisitos no funcionales", dtFechaCreacion = DateTime.Now }
			};
		}



		private async Task OnSubmint()
		{
			Loading.Show();

			var result = await Data.PostSaveRespuestas();
			ShowSnake(result);
			if (result.bResult)
			{
				await OnRespuestasSaved.InvokeAsync();
			}
			Loading.Hide();

		}

	}
}
