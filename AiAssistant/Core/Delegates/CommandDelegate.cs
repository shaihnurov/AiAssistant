using AiAssistant.Core.Models;
using System.Threading.Tasks;

namespace AiAssistant.Core.Delegates
{
    public delegate Task<bool> CommandDelegate(AssistantCommand command);
}
