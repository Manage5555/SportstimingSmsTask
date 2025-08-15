namespace SportstimingSmsTask
{
    interface SmsSender
    {
        Task<string> SendSmsAsync(string message, string from, string to);
        Task<string> SendSmsAsync(string message, string from, List<string> to);
        Task<string> CheckBalanceAsync();
    }
}
