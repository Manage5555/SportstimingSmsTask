using SportstimingSmsTask;

class SendSmsCommandline
{
    static void Main(string[] args)
    {
        string userName = "";
        string apiKey = "";
        SportstimingSmsSender smsSender = new SportstimingSmsSender(userName, apiKey);

        string response = smsSender.SendSms(args[0], args[1], args[2]);
        Console.WriteLine(response);
    }
}