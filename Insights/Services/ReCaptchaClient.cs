using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hcb.Insights.Services
{
    public class ReCaptchaResponse
    {
        [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public bool Success { get; set; }
        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("hostname")]
        public string TokenHostName { get; set; }
        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
    public class ReCaptchaClient
    {
        static readonly HttpClient httpClient = new HttpClient();
        internal static async Task<bool> Verify(string token, string hostName)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //            IConfigurationSection config = Startup.Configuration.GetSection("hcb");
            //            string secret = config.GetValue<string>("reCaptchaSecret");
            string secret = Environment.GetEnvironmentVariable("hcb:reCaptchaSecret");
            FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", Crypto.Decrypt(secret)),
                new KeyValuePair<string, string>("response", token)
            });
            HttpResponseMessage responseMsg = await httpClient.PostAsync(" https://www.google.com/recaptcha/api/siteverify", formContent);
            if (responseMsg.IsSuccessStatusCode == false)
            {
                return false;
            }
            string response = await responseMsg.Content.ReadAsStringAsync();
            ReCaptchaResponse reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(response);
            if (reCaptchaResponse.Success == false)
            {
                return false;
            }
            if (reCaptchaResponse.TokenHostName != hostName)
            {
                return false;
            }
            return true;
        }
    }
}
