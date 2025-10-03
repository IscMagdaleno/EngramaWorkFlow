using OpenAI.Chat;

using System.Text.Json;

using WorkFlow.API.EngramaLevels.Dominio.Servicios.Modelos;
using WorkFlow.API.EngramaLevels.Dominio.Servicios.Utiles;
using WorkFlow.Share.Objetos.Planes;

namespace WorkFlow.API.EngramaLevels.Dominio.Servicios
{
	public interface ILLMModuleGenerator
	{
		Task<List<Modulo>> GenerateModules(string titulo, string descripcion);
	}

	public class LLMModuleGenerator : ILLMModuleGenerator
	{
		private readonly IAzureIAService _azureIAService;

		public LLMModuleGenerator(IAzureIAService azureIAService)
		{
			_azureIAService = azureIAService;
		}

		public async Task<List<Modulo>> GenerateModules(string titulo, string descripcion)
		{
			// System Prompt simplificado para reducir riesgo de filtrado
			var systemPrompt = GeneraPrompts.GeneraModulosPrompt();

			// Prompt del usuario
			var userPrompt = $@"
			Título: {titulo}

			Descripción: {descripcion}

			Desglosa la aplicación en módulos funcionales en formato JSON válido, siguiendo las reglas proporcionadas.";

			var request = new RequestOpenAI
			{
				Configuration = systemPrompt,
				Prompt = userPrompt
			};

			try
			{
				// Llamar al servicio en modo JSON
				ChatCompletion completion = await _azureIAService.CallAzureOpenIAJson(request);

				// Validar que la respuesta sea JSON válido
				var jsonResponse = completion.Content[0].Text;
				try
				{
					var modulo = JsonSerializer.Deserialize<PlanTrabajo>(jsonResponse);
					return modulo.LstModulos;
				}
				catch (JsonException)
				{
					throw new Exception("La respuesta del LLM no es un JSON válido.");
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("content_filter"))
				{
					throw new Exception(
						"El prompt fue bloqueado por el filtro de contenido de Azure OpenAI. " +
						"Revisa el título y descripción o consulta https://go.microsoft.com/fwlink/?linkid=2198766");
				}
				throw;
			}
		}
	}
}