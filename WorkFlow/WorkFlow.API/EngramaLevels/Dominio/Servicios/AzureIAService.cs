using Azure;
using Azure.AI.OpenAI;

using OpenAI.Chat;

using WorkFlow.API.EngramaLevels.Dominio.Servicios.Modelos;
using WorkFlow.Share.Objetos.Proceso;
namespace WorkFlow.API.EngramaLevels.Dominio.Servicios
{
	public interface IAzureIAService
	{
		Task<ChatCompletion> CallAzureOpenIA(RequestOpenAI request);
		Task<ChatCompletion> CallAzureOpenIAJson(RequestOpenAI request);
		Task<ChatCompletion> CallAzureOpenIAWithMemory(Chat request);
	}

	public class AzureIAService : IAzureIAService
	{
		private readonly AzureOpenAIClient _azureClient;
		private readonly ChatClient _chatClient;

		public AzureIAService(IConfiguration configuration)
		{
			var endpoint = new Uri(configuration["AzureOpenAI:Endpoint"]);
			var apiKey = configuration["AzureOpenAI:ApiKey"];
			var deploymentName = configuration["AzureOpenAI:DeploymentName"] ?? "gpt-4.1";

			_azureClient = new(endpoint, new AzureKeyCredential(apiKey));
			_chatClient = _azureClient.GetChatClient(deploymentName);
		}


		public async Task<ChatCompletion> CallAzureOpenIAWithMemory(Chat chat)
		{

			var messages = new List<ChatMessage>();
			// Agregar historial de BD
			foreach (var msg in chat.LstMensajes)
			{
				if (msg.nvchRol == "system")
					messages.Add(new SystemChatMessage(msg.nvchContenido));
				else if (msg.nvchRol == "user")
					messages.Add(new UserChatMessage(msg.nvchContenido));
				else if (msg.nvchRol == "assistant")
					messages.Add(new AssistantChatMessage(msg.nvchContenido));
			}


			var options = new ChatCompletionOptions
			{
				Temperature = 1.0f,
				TopP = 1.0f,
				FrequencyPenalty = 0.0f,
				PresencePenalty = 0.0f,
			};



			var response = await _chatClient.CompleteChatAsync(messages, options);

			return response.Value;
		}


		public async Task<ChatCompletion> CallAzureOpenIA(RequestOpenAI request)
		{
			var options = new ChatCompletionOptions
			{
				Temperature = 1.0f,
				TopP = 1.0f,
				FrequencyPenalty = 0.0f,
				PresencePenalty = 0.0f,
			};

			var messages = new List<ChatMessage>
			{
				new SystemChatMessage(request.Configuration),
				new UserChatMessage(request.Prompt)
			};

			var response = await _chatClient.CompleteChatAsync(messages, options);

			return response.Value;
		}

		public async Task<ChatCompletion> CallAzureOpenIAJson(RequestOpenAI request)
		{
			var options = new ChatCompletionOptions
			{
				Temperature = 1.0f,
				TopP = 1.0f,
				FrequencyPenalty = 0.0f,
				PresencePenalty = 0.0f,
				ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
			};

			var messages = new List<ChatMessage>
			{
				new SystemChatMessage(request.Configuration),
				new UserChatMessage(request.Prompt)
			};

			var response = await _chatClient.CompleteChatAsync(messages, options);

			return response.Value;
		}
	}
}
