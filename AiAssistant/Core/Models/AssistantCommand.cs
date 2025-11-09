using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiAssistant.Core.Models
{
    /// <summary>
    /// Represents a command returned by the AI assistant.
    /// </summary>
    public class AssistantCommand
    {
        /// <summary>
        /// The name of the command/action.
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// The target or subject of the command.
        /// </summary>
        [JsonPropertyName("target")]
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// Additional parameters for the command, represented as a JSON element.
        /// </summary>
        [JsonPropertyName("parameters")]
        public JsonElement Parameters { get; set; }
    }
}