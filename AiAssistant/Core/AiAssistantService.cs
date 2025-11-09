using AiAssistant.Core.Delegates;
using AiAssistant.Core.Extensions;
using AiAssistant.Core.Models;
using AiAssistant.Engines;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AiAssistant.Core
{
    /// <summary>
    /// Core AI Assistant service that processes user queries, executes registered commands, and communicates with AI engines.
    /// </summary>
    public class AiAssistantService(ILogger<AiAssistantService> logger, IPromptBuilder promptBuilder) : IAiAssistantService
    {
        #region Properties
        /// <summary>
        /// The AI engine used for processing queries. Can be Groq or OpenAI.
        /// </summary>
        private IAssistantEngine? _engine;

        /// <summary>
        /// Dictionary of registered commands and their corresponding actions.
        /// Key: command name (case-insensitive), Value: delegate to execute the command.
        /// </summary>
        private readonly Dictionary<string, CommandDelegate> _commands = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// List of currently available command names.
        /// </summary>
        private readonly List<string> _availableCommands = [];

        /// <summary>
        /// Additional prompt information for commands.
        /// Key: command name (case-insensitive), Value: prompt instructions.
        /// </summary>
        private readonly Dictionary<string, string> _commandPrompts = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The base prompt describing the AI assistant's role.
        /// </summary>
        private readonly string _basePrompt = "You are an AI assistant for applications.";
        #endregion

        /// <inheritdoc/>
        public IReadOnlyList<string> GetAvailableCommands() => _availableCommands;

        /// <inheritdoc/>
        public void AddCommandPrompt(string commandName, string prompt) => _commandPrompts[commandName.Trim().ToLowerInvariant()] = prompt;

        /// <inheritdoc/>
        public void RegisterCommand(string commandName, CommandDelegate action)
        {
            var key = commandName.Trim().ToLowerInvariant();
            _commands[key] = action;
            if (!_availableCommands.Contains(key))
                _availableCommands.Add(key);
        }

        /// <inheritdoc/>
        public async Task<AssistantResponse> ProcessAsync(string query)
        {
            if (_engine == null)
                throw new InvalidOperationException("AI Engine is not configured. Call UseGroqEngine or UseOpenAIEngine first.");

            var userInstruction = BuildPrompt(query);
            var json = await _engine.ProcessQueryAsync(query, _availableCommands, userInstruction);
            logger.LogInformation("AI engine response: {Json}", json);

            var command = ParseCommand(json);
            var actionKey = (command.Action ?? string.Empty).Trim().ToLowerInvariant();

            if (!_commands.TryGetValue(actionKey, out var action))
            {
                logger.LogWarning("Command '{ActionKey}' not found.", actionKey);
                return new AssistantResponse
                {
                    Success = false,
                    Message = "Unknown command"
                };
            }

            var success = await action.Invoke(command);
            return new AssistantResponse
            {
                Success = success,
                Message = success ? "Command executed successfully" : "Command execution failed"
            };
        }

        /// <inheritdoc/>
        public void UseGroqEngine(string apiKey) => _engine = new GroqEngine(apiKey);

        /// <inheritdoc/>
        public void UseOpenAIEngine(string apiKey) => _engine = new OpenAIEngine(apiKey);

        /// <summary>
        /// Builds the full prompt to send to the AI engine, including base prompt, available commands, and user query.
        /// </summary>
        /// <param name="userQuery">The user's input query.</param>
        /// <returns>The generated prompt string.</returns>
        private string BuildPrompt(string userQuery) => promptBuilder.Build(_availableCommands, userQuery, _basePrompt, _commandPrompts);

        /// <summary>
        /// Parses the JSON returned by the AI engine into an AssistantCommand object.
        /// </summary>
        /// <param name="json">JSON string returned from AI engine.</param>
        /// <returns>Deserialized AssistantCommand. Defaults to "unknown" if parsing fails.</returns>
        private static AssistantCommand ParseCommand(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<AssistantCommand>(json) ?? new AssistantCommand { Action = "unknown" };
            }
            catch
            {
                return new AssistantCommand { Action = "unknown" };
            }
        }
    }
}