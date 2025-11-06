using AiAssistant.Core;
using AiAssistant.Engines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AiAssistant.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAiAssistant<TEngine>(this IServiceCollection services, Action<IAiAssistantService>? configure = null,
            Func<IServiceProvider, TEngine>? engineFactory = null) where TEngine : class, IAssistantEngine
        {
            if (engineFactory != null)
                services.AddSingleton<IAssistantEngine>(sp => engineFactory(sp)!);
            else
                services.AddSingleton<IAssistantEngine, TEngine>();

            services.AddSingleton<IAiAssistantService>(sp =>
            {
                var engine = sp.GetRequiredService<IAssistantEngine>();
                var logger = sp.GetRequiredService<ILogger<AiAssistantService>>();
                var assistant = new AiAssistantService(engine, logger);
                configure?.Invoke(assistant);
                return assistant;
            });

            return services;
        }
    }
}