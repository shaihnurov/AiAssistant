using GroqApiLibrary;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AiAssistant.Engines
{
    /// <summary>
    /// Implementation of <see cref="IAssistantEngine"/> using Groq API.
    /// </summary>
    public class GroqEngine(string apiKey) : IAssistantEngine
    {
        private readonly GroqApiClient _client = new(apiKey);

        /// <inheritdoc/>
        public async Task<string> ProcessQueryAsync(string query, IEnumerable<string> availableCommands, string userInstruction)
        {
            var request = new JsonObject
            {
                ["model"] = "llama-3.3-70b-versatile",
                ["messages"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["role"] = "user",
                        ["content"] = userInstruction
                    }
                }
            };

            var response = await _client.CreateChatCompletionAsync(request);
            var text = response?["choices"]?[0]?["message"]?["content"]?.ToString() ?? "";

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