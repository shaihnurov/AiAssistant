using AiAssistant.Core.Delegates;
using AiAssistant.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiAssistant.Core
{
    /// <summary>
    /// Represents a core AI assistant service that can process user queries, register commands, and communicate with AI engines.
    /// </summary>
    public interface IAiAssistantService
    {
        /// <summary>
        /// Gets the list of currently available command names.
        /// </summary>
        /// <returns>A read-only list of command names.</returns>
        IReadOnlyList<string> GetAvailableCommands();

        /// <summary>
        /// Adds additional prompt information for a specific command.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="prompt">Additional instructions for the command.</param>
        void AddCommandPrompt(string commandName, string prompt);

        /// <summary>
        /// Registers a command and its associated action.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="action">The delegate to execute the command.</param>
        void RegisterCommand(string commandName, CommandDelegate action);

        /// <summary>
        /// Processes a user query, executes the corresponding command if recognized, and returns a response.
        /// </summary>
        /// <param name="query">The user input query.</param>
        /// <returns>An <see cref="AssistantResponse"/> indicating the result of processing.</returns>
        Task<AssistantResponse> ProcessAsync(string query);

        /// <summary>
        /// Configures the AI assistant to use the Groq AI engine.
        /// </summary>
        /// <param name="apiKey">API key for Groq engine.</param>
        void UseGroqEngine(string apiKey);

        /// <summary>
        /// Configures the AI assistant to use the OpenAI engine.
        /// </summary>
        /// <param name="apiKey">API key for OpenAI engine.</param>
        void UseOpenAIEngine(string apiKey);
    }
}