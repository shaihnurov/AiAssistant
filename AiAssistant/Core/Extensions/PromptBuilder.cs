using System.Collections.Generic;
using System.Text;

namespace AiAssistant.Core.Extensions
{
    /// <summary>
    /// Default implementation of <see cref="IPromptBuilder"/> that constructs AI assistant prompts.
    /// </summary>
    public class PromptBuilder : IPromptBuilder
    {
        /// <inheritdoc/>
        public string Build(IEnumerable<string> availableCommands, string userQuery, string basePrompt, Dictionary<string, string> commandPrompts)
        {
            var sb = new StringBuilder();
            sb.AppendLine(basePrompt);
            sb.AppendLine($"Available commands: {string.Join(", ", availableCommands)}");

            foreach (var kv in commandPrompts)
                sb.AppendLine($"Additional info for command '{kv.Key}': {kv.Value}");

            sb.AppendLine("You must return JSON with only one of these commands. " +
                          "If the user writes something unrelated to the commands, return action='unknown'.");
            sb.AppendLine("JSON format: {\"action\":\"<command>\",\"target\":\"<target>\",\"parameters\":\"<parameters>\"}");
            sb.AppendLine($"User query: {userQuery}");

            return sb.ToString();
        }
    }
}