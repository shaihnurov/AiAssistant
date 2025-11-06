using AiAssistant.Core;
using AiAssistant.DependencyInjection;
using AiAssistant.Engines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoConsole
{
    public class Program
    {
        private static IAiAssistantService? _assistantService;

        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            services.AddAiAssistant<GroqEngine>(assistant =>
            {
                assistant.RegisterCommand("openpage", async cmd =>
                {
                    Console.WriteLine($"Открываю страницу: {cmd.Target}");
                    return await Task.FromResult(true);
                });

                assistant.RegisterCommand("search", async cmd =>
                {
                    Console.WriteLine($"Ищу: {cmd.Parameters}");
                    return true;
                });

                assistant.RegisterCommand("exit", async cmd =>
                {
                    Environment.Exit(0);
                    return true;
                });
            }, sp => new GroqEngine(""));

            var provider = services.BuildServiceProvider();
            _assistantService = provider.GetRequiredService<IAiAssistantService>();

            while (true)
            {
                await Messages();
            }
        }

        private static async Task Messages()
        {
            Console.WriteLine("Введите сообщение для AI-ассистента (или 'exit' для выхода):");

            var msg = Console.ReadLine();

            var response = await _assistantService!.ProcessAsync(msg!);
            Console.WriteLine(response.Message);
        }
    }
}
