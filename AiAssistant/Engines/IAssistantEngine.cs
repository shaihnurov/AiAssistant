using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiAssistant.Engines
{
    /// <summary>
    /// Defines the interface for AI engines that process user queries and return JSON commands.
    /// </summary>
    public interface IAssistantEngine
    {
        /// <summary>
        /// Processes a user query and returns a JSON string representing the chosen command.
        /// </summary>
        /// <param name="query">The user's input query.</param>
        /// <param name="availableCommands">List of available command names.</param>
        /// <param name="userInstruction">The instruction/prompt to guide the AI.</param>
        /// <returns>A JSON string representing the assistant's command response.</returns>
        Task<string> ProcessQueryAsync(string query, IEnumerable<string> availableCommands, string userInstruction);
    }
}