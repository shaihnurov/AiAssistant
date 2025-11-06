using AiAssistant.Core.Models;

namespace AiAssistant.Core.Delegates
{
    public delegate Task<bool> CommandDelegate(AssistantCommand command);
}
