using AiAssistant.Core.Delegates;
using AiAssistant.Core.Models;
using AiAssistant.Engines;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AiAssistant.Core
{
    public class AiAssistantService(IAssistantEngine engine, ILogger<AiAssistantService> logger) : IAiAssistantService
    {
        public readonly Dictionary<string, CommandDelegate> _commands = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<string> GetAvailableCommands() => [.. _commands.Keys];

        /// <summary>
        /// Регистрирует команду по имени и её действие
        /// </summary>
        public void RegisterCommand(string commandName, CommandDelegate action)
        {
            _commands[commandName.Trim().ToLowerInvariant()] = action;
        }

        public async Task<AssistantResponse> ProcessAsync(string query)
        {
            var availableCommands = GetAvailableCommands();

            var json = await engine.ProcessQueryAsync(query, availableCommands);
            logger.LogInformation("LLM ответ: " + json);

            var command = JsonSerializer.Deserialize<AssistantCommand>(json);

            if (command == null)
            {
                return new AssistantResponse
                {
                    Success = false,
                    Message = "Неизвестная команда (JSON не удалось распознать)"
                };
            }

            // Надёжная очистка строки action
            var actionKey = (command.Action ?? string.Empty).Trim().ToLowerInvariant();

            if (!_commands.TryGetValue(actionKey, out var action))
            {
                Console.WriteLine($"Команда '{command.Action}' не найдена в зарегистрированных.");
                return new AssistantResponse
                {
                    Success = false,
                    Message = "Неизвестная команда"
                };
            }

            var success = await action.Invoke(command);
            return new AssistantResponse
            {
                Success = success,
                Message = success ? "Команда выполнена" : "Ошибка выполнения"
            };
        }
    }
}
