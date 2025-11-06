using AiAssistant.Core.Models;

namespace AiAssistant.Core
{
    public interface IAiAssistantService
    {
        /// <summary>
        /// Регистрирует команду по имени и её действие
        /// </summary>
        void RegisterCommand(string commandName, Delegates.CommandDelegate action);

        /// <summary>
        /// Обрабатывает пользовательский запрос через LLM и выполняет соответствующую команду
        /// </summary>
        Task<AssistantResponse> ProcessAsync(string query);
    }
}
