using AiAssistant.Core;
using AiAssistant.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AiAssistant.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register AI Assistant services into an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers AI Assistant services, including <see cref="IPromptBuilder"/> and <see cref="IAiAssistantService"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configure">
        /// Optional configuration action to register commands or customize the assistant after creation.
        /// </param>
        /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
        public static IServiceCollection AddAiAssistant(
            this IServiceCollection services,
            Action<IAiAssistantService>? configure = null)
        {
            // Register the PromptBuilder implementation
            services.AddSingleton<IPromptBuilder, PromptBuilder>();

            // Register the AI Assistant service
            services.AddSingleton<IAiAssistantService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<AiAssistantService>>();
                var promptBuilder = sp.GetRequiredService<IPromptBuilder>();
                var assistant = new AiAssistantService(logger, promptBuilder);

                // Allow additional configuration (registering commands)
                configure?.Invoke(assistant);

                return assistant;
            });

            return services;
        }
    }
}