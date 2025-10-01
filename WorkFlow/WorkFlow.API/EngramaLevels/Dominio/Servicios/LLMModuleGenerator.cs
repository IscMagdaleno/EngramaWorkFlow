using OpenAI.Chat;

using System.Text.Json;

using WorkFlow.API.EngramaLevels.Dominio.Servicios.Modelos;
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
			var systemPrompt = @"
				Eres un arquitecto de software experto en aplicaciones web con .NET, Clean Architecture, SQL Server y Blazor/MudBlazor. Tu tarea es desglosar aplicaciones en módulos funcionales según Domain-Driven Design (DDD) y principios SOLID.

				Reglas:
				- Identifica módulos lógicos basados en el propósito de la app.
				- Cada módulo debe tener:
				  - ""vchTitulo"": Nombre claro del módulo (en español).
				  - ""nvchProposito"": Propósito del módulo, explicando su rol en la app.
				  - ""Funcionalidades"": Lista de funcionalidades, cada una con:
					- Descripción de la funcionalidad.
					- Entidades involucradas (e.g., User, Order).
					- Interacciones con otros módulos.
					- Detalles técnicos: Backend (.NET), DB (SQL Server), Frontend (MudBlazor).
					- Seguridad, escalabilidad, pruebas.
				- Genera salida en JSON válido con clave ""modules"".
				- Evita contenido sensible o complejo que pueda activar filtros de contenido.

				Ejemplo:
				{
				  ""LstModulos"": [
					{
					  ""vchTitulo"": ""Gestión de Usuarios"",
					  ""nvchProposito"": ""Administra usuarios según Clean Architecture."",
					  ""LstFuncionalidades"": [
						{
						  ""nvchDescripcion"": ""Crear usuario."",
						  ""nvchEntidades"": ""User, Roles"",
						  ""nvchInteracciones"": ""Consulta Módulo de Autenticación."",
						  ""nvchTecnico"": ""Backend: Mediator; DB: Tabla Users; Frontend: MudForm."",
						  ""nvchConsideraciones"": ""Usar JWT; pruebas unitarias.""
						}
					  ]
					}
				  ]
				}";

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