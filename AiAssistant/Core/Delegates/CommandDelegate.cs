using AiAssistant.Core.Models;
using System.Threading.Tasks;

namespace AiAssistant.Core.Delegates
{
    /// <summary>
    /// Delegate representing an asynchronous command execution.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <returns>True if execution succeeded; otherwise false.</returns>
    public delegate Task<bool> CommandDelegate(AssistantCommand command);
}