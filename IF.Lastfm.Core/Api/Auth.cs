using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private const string ApiAuthMethod = "auth.getMobileSession";

        private readonly string _apiSecret;
        private string _password;
        private string _username;

        public bool HasAuthenticated { get { return User != null; } }
        public string ApiKey { get; private set; }
        public UserSession User { get; private set; }

        public Auth(string apikey, string secret)
        {
            ApiKey = apikey;
            _apiSecret = secret;
        }

        /// <summary>
        /// Load an existing user session
        /// </summary>
        /// <param name="session">Session to load</param>
        /// <returns>Whether session object is valid</returns>
        public bool LoadSession(UserSession session)
        {
            User = session;
            return true;
        }

        public async Task<LastResponse> GetSessionTokenAsync(string username, string password)
        {
            const string apiMethod = "auth.getMobileSession";

            _password = password;
            _username = username;

            var apisig = GenerateMethodSignature(apiMethod);

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

        public string GenerateMethodSignature(string method, Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            parameters.Add("api_key", ApiKey);
            parameters.Add("method", method);
            parameters.Add("password", _password);
            parameters.Add("username", _username);

            var builder = new StringBuilder();

            foreach (var kv in parameters.OrderBy(kv => kv.Key))
            {
                builder.Append(kv.Key);
                builder.Append(kv.Value);
            }

            builder.Append(_apiSecret);

            return MD5.GetHashString(builder.ToString());
        }
    }
}