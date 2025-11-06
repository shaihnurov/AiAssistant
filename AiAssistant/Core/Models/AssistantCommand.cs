using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiAssistant.Core.Models
{
    public class AssistantCommand
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("target")]
        public string Target { get; set; } = string.Empty;

        [JsonPropertyName("parameters")]
        public JsonElement Parameters { get; set; }
    }
}