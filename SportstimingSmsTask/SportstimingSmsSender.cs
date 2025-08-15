using System.Text.Json;
using System.Text.Json.Nodes;

namespace SportstimingSmsTask
{
    internal class SportstimingSmsSender
    {
        private SmsSender smsSender;

        /// <summary>
        /// Object to send sms messages
        /// Currently using CPSMS as sms gateway
        /// </summary>
        /// <param name="userName">CPSMS username</param>
        /// <param name="apiKey">CPSMS api key</param>
        public SportstimingSmsSender(string userName, string apiKey)
        {
            smsSender = new CPSMSSender(userName, apiKey);
        }

        /// <summary>
        /// Sends a sms
        /// </summary>
        /// <param name="message">The body text of the SMS message</param>
        /// <param name="from">Number seen by the receiver</param>
        /// <param name="to">Receiving number</param>
        /// <returns>Returns the status result</returns>
        public string SendSms(string message, string from, string to)
        {
            return smsSender.SendSmsAsync(message, from, to).Result;
        }

        /// <summary>
        /// Sends a sms to multiple people
        /// </summary>
        /// <param name="message">The body text of the SMS message</param>
        /// <param name="from">Number seen by the receiver</param>
        /// <param name="to">List containing the numbers receiving the sms</param>
        /// <returns>Returns the status result</returns>
        public string SendSms(string message, string from, List<string> to)
        {
            return smsSender.SendSmsAsync(message, from, to).Result;
        }

        /// <summary>
        /// Sends a sms async
        /// </summary>
        /// <param name="message">The body text of the SMS message</param>
        /// <param name="from">Number seen by the receiver</param>
        /// <param name="to">Receiving number</param>
        /// <returns>Returns the status result</returns>
        public async Task<string> SendSmsAsync(string message, string from, string to)
        {
            return await smsSender.SendSmsAsync(message, from, to);
        }

        /// <summary>
        /// Sends a sms to multiple people async
        /// </summary>
        /// <param name="message">The body text of the SMS message</param>
        /// <param name="from">Number seen by the receiver</param>
        /// <param name="to">List containing the numbers receiving the sms</param>
        /// <returns>Returns the status result</returns>
        public async Task<string> SendSmsAsync(string message, string from, List<string> to)
        {
            return await smsSender.SendSmsAsync(message, from, to);
        }

        /// <summary>
        /// Checks the credits of the user account
        /// </summary>
        /// <remarks>Returns string because of the format that is returned by cpsms (Like "9.843,40")</remarks>
        /// <returns>Credit amout in string format</returns>
        public string CheckCredit()
        {
            string creditAsJsonString = smsSender.CheckBalanceAsync().Result;
            JsonObject creditAsJson = JsonSerializer.Deserialize<JsonObject>(creditAsJsonString);
            return creditAsJson["credit"].GetValue<string>();
        }
    }
}
