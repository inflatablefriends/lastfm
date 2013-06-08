using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xBrainLab.Security.Cryptography;

namespace IF.Lastfm.Core.Api
{
    public class Auth : IAuth
    {
        private const string ApiSignatureSeedFormat = "api_key{0}method{1}password{2}username{3}{4}";
        private const string ApiAuthMethod = "auth.getMobileSession";

        protected string ApiSecret { get; set; }
        public string ApiKey { get; set; }

        public Auth(string apikey, string secret)
        {
            ApiKey = apikey;
            ApiSecret = secret;
        }

        public async Task<UserSession> GetSessionTokenAsync(string username, string password)
        {
            const string apiMethod = "auth.getMobileSession";

            var apisigseed = string.Format(ApiSignatureSeedFormat, ApiKey, ApiAuthMethod, password, username, ApiSecret);

            var apisig = MD5.GetHashString(apisigseed);

            var postContent = LastFm.GetPostBody(apiMethod, ApiKey, apisig, new Dictionary<string, string>
                                                                                {
                                                                                    {"password", password},
                                                                                    {"username", username}
                                                                                });

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("https://ws.audioscrobbler.com/2.0/", postContent);
            var json = await response.Content.ReadAsStringAsync();
            var sessionObject = JsonConvert.DeserializeObject<JObject>(json).GetValue("session");

            var session = JsonConvert.DeserializeObject<UserSession>(sessionObject.ToString());

            return session;
        }
    }
}