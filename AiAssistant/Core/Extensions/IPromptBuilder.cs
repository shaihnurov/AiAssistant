using System.Collections.Generic;

namespace AiAssistant.Core.Extensions
{
    /// <summary>
    /// Interface for building prompts for the AI assistant.
    /// </summary>
    public interface IPromptBuilder
    {
        /// <summary>
        /// Builds a prompt string including the base prompt, available commands, additional command info, and the user query.
        /// </summary>
        /// <param name="availableCommands">List of available command names.</param>
        /// <param name="userQuery">The user's input query.</param>
        /// <param name="basePrompt">The base prompt describing the AI assistant's role.</param>
        /// <param name="commandPrompts">Dictionary of additional instructions for specific commands.</param>
        /// <returns>The generated prompt string.</returns>
        string Build(IEnumerable<string> availableCommands, string userQuery, string basePrompt, Dictionary<string, string> commandPrompts);
    }
}