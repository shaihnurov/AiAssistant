using AiAssistant.Core.Models;
using System.Text.Json;

namespace AiAssistant.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="AssistantCommand"/> to simplify parameter retrieval.
    /// </summary>
    public static class AssistantCommandExtensions
    {
        /// <summary>
        /// Retrieves a typed parameter value from an <see cref="AssistantCommand"/>.
        /// Returns <paramref name="defaultValue"/> if the key does not exist or the parameters are not a JSON object.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the parameter into.</typeparam>
        /// <param name="cmd">The <see cref="AssistantCommand"/> instance.</param>
        /// <param name="key">The parameter key to retrieve.</param>
        /// <param name="defaultValue">The default value to return if the parameter is not found.</param>
        /// <returns>The deserialized parameter value or <paramref name="defaultValue"/>.</returns>
        public static T GetParameter<T>(this AssistantCommand cmd, string key, T defaultValue = default!)
        {
            if (cmd.Parameters.ValueKind != JsonValueKind.Object) return defaultValue;
            if (!cmd.Parameters.TryGetProperty(key, out var prop)) return defaultValue;

            return JsonSerializer.Deserialize<T>(prop.GetRawText())!;
        }
    }
}