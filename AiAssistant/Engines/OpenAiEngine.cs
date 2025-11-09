using OpenAI;
using OpenAI.Chat;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AiAssistant.Engines
{
    /// <summary>
    /// Implementation of <see cref="IAssistantEngine"/> using OpenAI GPT models.
    /// </summary>
    public class OpenAIEngine : IAssistantEngine
    {
        private readonly ChatClient _chatClient;

        /// <summary>
        /// Initializes a new instance of <see cref="OpenAIEngine"/> with the specified API key.
        /// </summary>
        /// <param name="apiKey">OpenAI API key.</param>
        public OpenAIEngine(string apiKey)
        {
            var client = new OpenAIClient(apiKey);
            _chatClient = client.GetChatClient("gpt-4o-mini");
        }

        /// <inheritdoc/>
        public async Task<string> ProcessQueryAsync(string query, IEnumerable<string> availableCommands, string userInstruction)
        {
            ChatMessage[] messages =
            [
                ChatMessage.CreateSystemMessage(userInstruction),
                ChatMessage.CreateUserMessage(query)
            ];

            var completion = await _chatClient.CompleteChatAsync(messages);
            var text = completion.Value.Content[0].Text.Trim();

            try
            {
                JsonDocument.Parse(text);
                return text;
            }
            catch
            {
                return JsonSerializer.Serialize(new { action = "unknown", target = "", parameters = "" });
            }
        }
    }
}