using AiAssistant.Core;
using AiAssistant.DependencyInjection;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoConsole
{
    public class Program
    {
        private static IAiAssistantService? _assistantService;

        public static async Task Main()
        {
            Env.Load();

            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            // Register AI Assistant services in DI container
            services.AddAiAssistant();

            var provider = services.BuildServiceProvider();
            _assistantService = provider.GetRequiredService<IAiAssistantService>();

            string groqApiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? throw new InvalidOperationException("GROQ_API_KEY не задан");

            // Configure the AI Assistant to use the Groq engine
            _assistantService.UseGroqEngine(groqApiKey);

            // Register the "openpage" command with its action
            _assistantService.RegisterCommand("openpage", async cmd =>
            {
                Console.WriteLine($"Opening page: {cmd.Target}");

                return await Task.FromResult(true);
            });

            // Provide additional prompt instructions for the "openpage" command
            _assistantService.AddCommandPrompt("openpage", "Available pages: Print - PrintPage, Home - HomePage");

            // Register the "find" command for searching items
            _assistantService.RegisterCommand("find", async cmd =>
            {
                Console.WriteLine($"Searching for: {cmd.Parameters}");
                return true;
            });

            // Register the "exit" command to terminate the application
            _assistantService.RegisterCommand("exit", async cmd =>
            {
                Environment.Exit(0);
                return true;
            });

            while (true)
            {
                await Messages();
            }
        }

        private static async Task Messages()
        {
            Console.Write("Enter a message for the AI Assistant (or 'exit' to quit): ");

            var msg = Console.ReadLine();

            // Process the input using the AI Assistant service
            var response = await _assistantService!.ProcessAsync(msg!);
            Console.WriteLine(response.Message);
        }
    }
}