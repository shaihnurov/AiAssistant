using OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace AiAssistant.Engines
{
    public class OpenAiEngine : IAssistantEngine
    {
        private readonly ChatClient _chatClient;

        public OpenAiEngine(string apiKey)
        {
            var client = new OpenAIClient(apiKey);
            _chatClient = client.GetChatClient("gpt-4o-mini");
        }

        public async Task<string> ProcessQueryAsync(string query, IEnumerable<string> availableCommands)
        {
            // Сообщения для чата
            ChatMessage[] messages =
            {
                ChatMessage.CreateSystemMessage(
                    "Ты — AI-ассистент для десктоп-приложений. " +
                    "Отвечай ТОЛЬКО в формате JSON: {\"action\":\"string\", \"target\":\"string\", \"parameters\":\"string\"}. " +
                    "Не добавляй пояснений или текста вне JSON."),
                ChatMessage.CreateUserMessage(query)
            };

            // Отправляем запрос
            var completion = await _chatClient.CompleteChatAsync(messages);

            // Получаем ответ модели
            var text = completion.Value.Content[0].Text.Trim();

            // Проверяем, что это JSON
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