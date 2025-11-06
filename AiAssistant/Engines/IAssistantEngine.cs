using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiAssistant.Engines
{
    public interface IAssistantEngine
    {
        Task<string> ProcessQueryAsync(string query, IEnumerable<string> availableCommands);
    }
}