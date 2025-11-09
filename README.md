# AiAssistant

[![NuGet](https://img.shields.io/nuget/v/AiAssistant.svg)](https://www.nuget.org/packages/AiAssistant/)  

**AiAssistant** — This is a lightweight and extensible library for integrating AI assistants into .NET applications. The library allows you to process user requests, register commands, work with various AI engines (Groq, OpenAI), and centrally generate prompts.

---

## Features

- Registration of user commands and handling of their execution via delegates.
- Support for multiple AI engines:
- [Groq API](https://www.groq.com/)
- [OpenAI GPT](https://platform.openai.com/)
- Prompt generation with centralized logic via `IPromptBuilder`.
- Extensions for convenient command parameter extraction.
- Full support for asynchronous interaction.
- Easy integration via Dependency Injection (`Microsoft.Extensions.DependencyInjection`).

---

## Installation

Install via NuGet:

```bash
dotnet add package AiAssistant --version 0.0.3
```

## Usage

1. Register the service in your DI container
   ```csharp
   using Microsoft.Extensions.DependencyInjection;
   using AiAssistant.DependencyInjection;

   var services = new ServiceCollection();

   // Register AI Assistant services in DI container
   services.AddAiAssistant();
   
   var serviceProvider = services.BuildServiceProvider();
   ```
2. How to use
   ```csharp
    using AiAssistant.Core;
    
    var assistantService = provider.GetRequiredService<IAiAssistantService>();
    
    // Select the desired engine and submit the API key
    assistantService.UseGroqEngine(groqApiKey);
    // Or
    assistantService.UseOpenAIEngine(openAI-ApiKey);
    
    // Example of command registration
    // Registration of the “openpage” command with action
    _assistantService.RegisterCommand("openpage", async cmd =>
    {
        Console.WriteLine($"Opening page: {cmd.Target}");
        return await Task.FromResult(true);
    });

    // Provide additional prompt instructions for the "openpage" command
    _assistantService.AddCommandPrompt("openpage", "Available pages: Print - PrintPage, Home - HomePage");

    // Registration of the “find” command with action
    _assistantService.RegisterCommand("find", async cmd =>
    {
        Console.WriteLine($"Searching for: {cmd.Parameters}");
        return true;
    });

    // Registration of the “exit” command with action
    _assistantService.RegisterCommand("exit", async cmd =>
    {
        Environment.Exit(0);
        return true;
    });

    // Call
    Console.Write("Enter a message for the AI Assistant (or 'exit' to quit): ");

    var msg = Console.ReadLine();

    // Process the input using the AI Assistant service
    var response = await _assistantService!.ProcessAsync(msg!);
   ```

## API

 IAiAssistantService
 
  Main library interface:
   - RegisterCommand(string commandName, CommandDelegate action) — command registration.
   - AddCommandPrompt(string commandName, string prompt) — adding additional instructions for a command.
   - ProcessAsync(string query) — processing a user request.
   - GetAvailableCommands() — getting a list of available commands.
   - UseGroqEngine(string apiKey) — connecting the Groq AI engine.
   - UseOpenAIEngine(string apiKey) — connecting OpenAI GPT.

 AssistantCommandExtensions
   Extensions for conveniently obtaining command parameters:
```csharp
var value = command.GetParameter<int>("count", 0);
```

## Supported Platforms
 - .NET 9.0

## License
MIT License. See [LICENSE](LICENSE.txt) for details.

## Contributors

[See all contributors](https://github.com/shaihnurov/AiAssistant/graphs/contributors)
