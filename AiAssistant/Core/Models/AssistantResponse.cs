namespace AiAssistant.Core.Models
{
    /// <summary>
    /// Represents the response returned after executing an AI assistant command.
    /// </summary>
    public class AssistantResponse
    {
        /// <summary>
        /// Indicates whether the command execution was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message providing details about the execution result.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}