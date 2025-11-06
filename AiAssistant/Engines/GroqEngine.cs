using GroqApiLibrary;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiAssistant.Engines
{
    public class GroqEngine(string apiKey) : IAssistantEngine
    {
        private readonly GroqApiClient _client = new(apiKey);

        public async Task<string> ProcessQueryAsync(string query, IEnumerable<string> availableCommands)
        {
            var commandList = string.Join(", ", availableCommands);

            var userInstruction =
                $"Ты — AI-ассистент для десктоп-приложений.\n" +
                $"Доступные команды: {commandList}\n" +
                "Всегда отвечай строго в формате JSON:\n" +
                "{\"action\":\"<команда>\",\"target\":\"<target>\",\"parameters\":\"<parameters>\"}\n" +
                $"Пользовательский запрос: {query}";

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

            var text = response?["choices"]?[0]?["message"]?["content"]?.ToString() ?? "no Groq response";

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