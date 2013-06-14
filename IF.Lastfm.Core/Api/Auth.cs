using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xBrainLab.Security.Cryptography;

namespace IF.Lastfm.Core.Api
{
    public class Auth : IAuth
    {
        private const string ApiSignatureSeedFormat = "api_key{0}method{1}password{2}username{3}{4}";
        private const string ApiAuthMethod = "auth.getMobileSession";

        private readonly string _apiSecret;

        public bool HasAuthenticated { get { return User != null; } }
        public string ApiKey { get; private set; }
        public UserSession User { get; private set; }

        public Auth(string apikey, string secret)
        {
            ApiKey = apikey;
            _apiSecret = secret;
        }

        public async Task<LastResponse>  GetSessionTokenAsync(string username, string password)
        {
            const string apiMethod = "auth.getMobileSession";

            var apisigseed = string.Format(ApiSignatureSeedFormat, ApiKey, ApiAuthMethod, password, username, _apiSecret);

            var apisig = MD5.GetHashString(apisigseed);

            var postContent = LastFm.CreatePostBody(apiMethod, ApiKey, apisig, new Dictionary<string, string>
                                                                                {
                                                                                    {"password", password},
                                                                                    {"username", username}
                                                                                });

            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("https://ws.audioscrobbler.com/2.0/", postContent);
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var sessionObject = JsonConvert.DeserializeObject<JObject>(json).GetValue("session");

                User = JsonConvert.DeserializeObject<UserSession>(sessionObject.ToString());

                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse(error);
            }
        }
    }
}