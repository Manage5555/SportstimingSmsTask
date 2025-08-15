using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace SportstimingSmsTask
{
    internal class CPSMSSender : SmsSender
    {
        private static readonly string CPSMS_URL = "api.cpsms.dk/v2/";
        private static readonly string CPSMS_SINGLE_SMS_ENDPOINT = "send";
        private static readonly string CPSMS_GROUP_SMS_ENDPOINT = "sendgroup";
        private static readonly string CPSMS_SMS_CREDIT_ENDPOINT = "creditvalue";

        private string userName;
        private string apiKey;
        private HttpClient client;

        public CPSMSSender(string userName, string apiKey)
        {
            this.userName = userName;
            this.apiKey = apiKey;
            client = new HttpClient();
        }

        /// <summary>
        /// Sends the containing sms using CPSMS
        /// </summary>
        /// <param name="message">The body text of the SMS message</param>
        /// <param name="from">Number seen by the receiver. limit of 15 chars or 11 alphanumeric</param>
        /// <param name="to">Receiving number. Must start with country code</param>
        /// <returns>Returns the status result from CPSMS. See https://api.cpsms.dk/documentation/index.html?shell#send for details</returns>
        public async Task<string> SendSmsAsync(string message, string from, string to)
        {
            if(String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("from and to cannot be empty or null - from is: " + nameof(from) + " - to is: " + nameof(to));
            }

            HttpRequestMessage httpRequest = CPSMSHttpRequestSingleSms(message, from, to);
            HttpResponseMessage response = await client.SendAsync(httpRequest);

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Sends the containing sms using CPSMS
        /// </summary>
        /// <param name="message">The body text of the SMS message</param>
        /// <param name="from">Number seen by the receiver. limit of 15 chars or 11 alphanumeric</param>
        /// <param name="to">Receiving numbers. Each number must start with country code</param>
        /// <returns>Returns the status result from CPSMS. See https://api.cpsms.dk/documentation/index.html?shell#send for details</returns>
        public async Task<string> SendSmsAsync(string message, string from, List<string> to)
        {
            if (String.IsNullOrEmpty(from) || to == null || to.Count == 0)
            {
                throw new ArgumentNullException("from and to cannot be empty or null - from is: " + nameof(from) + " - to is: " + nameof(to));
            }

            HttpRequestMessage httpRequest = CPSMSHttpRequestGroupSms(message, from, to);
            HttpResponseMessage response = await client.SendAsync(httpRequest);

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Sends a request to check credit on the account
        /// </summary>
        /// <returns>Credit value in the form of a json object using the key "credit"</returns>
        public async Task<string> CheckBalanceAsync()
        {
            HttpRequestMessage httpRequest = CPSMSHttpRequestCredit();
            HttpResponseMessage response = await client.SendAsync(httpRequest);

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Returns a http request used for a single sms for CPSMS
        /// </summary>
        private HttpRequestMessage CPSMSHttpRequestSingleSms(string message, string from, string to)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, CPSMS_URL + CPSMS_SINGLE_SMS_ENDPOINT);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", userName + ":" + apiKey);

            JsonObject sms = SmsAsJsonObject(message, from, to);
            httpRequest.Content = new StringContent(sms.ToJsonString());

            return httpRequest;
        }

        /// <summary>
        /// Returns a http request used for group sms for CPSMS
        /// </summary>
        private HttpRequestMessage CPSMSHttpRequestGroupSms(string message, string from, List<string> to)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, CPSMS_URL + CPSMS_GROUP_SMS_ENDPOINT);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", userName + ":" + apiKey);

            JsonObject sms = SmsAsJsonObject(message, from, to);
            httpRequest.Content = new StringContent(sms.ToJsonString());

            return httpRequest;
        }

        /// <summary>
        /// Returns a http request used to check credits left on your account for CPSMS
        /// </summary>
        private HttpRequestMessage CPSMSHttpRequestCredit()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, CPSMS_URL + CPSMS_SMS_CREDIT_ENDPOINT);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", userName + ":" + apiKey);
            return httpRequest;
        }

        /// <summary>
        /// Returns a JsonObject, containing the sms needed for CPSMS http request
        /// </summary>
        private JsonObject SmsAsJsonObject(string message, string from, string to)
        {
            JsonObject sms = new JsonObject();
            sms.Add("message", message);
            sms.Add("from", from);
            sms.Add("to", to);

            return sms;
        }

        /// <summary>
        /// Returns a JsonObject, containing the sms needed for CPSMS http request
        /// </summary>
        private JsonObject SmsAsJsonObject(string message, string from, List<string> to)
        {
            JsonArray toArray = new JsonArray();
            foreach(string eachTo in to)
            {
                toArray.Add(eachTo);
            }

            JsonObject sms = new JsonObject();
            sms.Add("message", message);
            sms.Add("from", from);
            sms.Add("to_group", toArray);

            return sms;
        }
    }
}
