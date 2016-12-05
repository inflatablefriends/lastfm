using IF.Lastfm.Core.Api.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("IF.Lastfm.Core.Tests")]
[assembly: InternalsVisibleTo("IF.Lastfm.Core.Tests.Integration")]
[assembly: InternalsVisibleTo("IF.Lastfm.SQLite")]
[assembly: InternalsVisibleTo("IF.Lastfm.Syro")]
namespace IF.Lastfm.Core
{
    public static class LastFm
    {
        internal const string SCROBBLING_HELP_URL = "https://github.com/inflatablefriends/lastfm/blob/scrobbler/doc/scrobbling.md";
        internal const string TEST_APIKEY = "a6ab4b9376e54cdb06912bfbd9c1f288";
        internal const string TEST_APISECRET = "3aa7202fd1bc6d5a7ac733246cbccc4b";

        public const string ApiRoot = "http://ws.audioscrobbler.com/2.0/";
        public const string ApiRootSsl = "https://ws.audioscrobbler.com/2.0/";
        private const string ApiRootFormat = "{0}://ws.audioscrobbler.com/2.0/?method={1}&api_key={2}{3}";

        private const string ResponseFormat = "json";

        public const string DefaultLanguageCode = "en";
        public const int DefaultPageLength = 20;
        
        public static string FormatApiUrl(string method, string apikey, Dictionary<string, string> parameters = null, bool secure = false)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            parameters.Add("format", ResponseFormat);

            var querystring = LastFm.FormatQueryParameters(parameters.OrderBy(kv => kv.Key));

            var protocol = secure
                               ? "https"
                               : "http";

            return string.Format(ApiRootFormat, protocol, method, apikey, querystring);
        }

        public static FormUrlEncodedContent CreatePostBody(string method, string apikey, string apisig,
                                                          IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var init = new Dictionary<string, string>
                           {
                               {"method", method},
                               {"api_key", apikey},
                               {"api_sig", apisig},
                               {"format", ResponseFormat}
                           };

            // TODO ordering
            var requestParameters = init.Concat(parameters);

            return new FormUrlEncodedContent(requestParameters);
        }

        public static string FormatQueryParameters(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            const string parameterFormat = "&{0}={1}";

            var builder = new StringBuilder();

            foreach (var pair in parameters)
            {
                builder.Append(string.Format(parameterFormat, pair.Key, pair.Value));
            }

            return builder.ToString();
        }

        /// <summary>
        /// TODO see issue #5
        /// </summary>
        /// <param name="json">String of JSON</param>
        /// <param name="status">Enum indicating the Status, Unknown if there is no Status</param>
        /// <returns>True when the JSON could be parsed and it didn't describe a known Last.Fm Status.</returns>
        public static bool IsResponseValid(string json, out LastResponseStatus status)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                status = LastResponseStatus.Unknown;
                return false;
            }

            JObject jo;
            try
            {
                jo = JsonConvert.DeserializeObject<JObject>(json);
            }
            catch (JsonException)
            {
                status = LastResponseStatus.Unknown;
                return false;
            }

            var codeString = jo.Value<string>("error");
            if (string.IsNullOrWhiteSpace(codeString) && json.Length > 1)
            {
                status = LastResponseStatus.Successful;
                return true;
            }

            status = LastResponseStatus.Unknown;

            int code;
            if (Int32.TryParse(codeString, out code))
            {
                status = (LastResponseStatus) code;
            }

            return false;
        }
    }
}