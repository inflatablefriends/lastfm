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
[assembly: InternalsVisibleTo("IF.Lastfm.ProgressReport")]
namespace IF.Lastfm.Core
{
    public class LastFm
    {
        #region Constants

        public const string ApiRoot = "http://ws.audioscrobbler.com/2.0/";
        public const string ApiRootSsl = "https://ws.audioscrobbler.com/2.0/";
        private const string ApiRootFormat = "{0}://ws.audioscrobbler.com/2.0/?method={1}&api_key={2}{3}";

        private const string ResponseFormat = "json";

        public const string DefaultLanguageCode = "en";
        public const int DefaultPageLength = 20;
        
        #endregion
        
        /// <summary>
        /// Determines whether commands should throw HttpRequestExceptions or wrap them
        /// in the response.
        /// 
        /// Using this can make client code neater, but it violates the principles of 
        /// separation of concerns and single responsibility a bit. This property won't
        /// get removed but please only use it if you understand what it does!
        /// </summary>
        [Obsolete]
        public static bool CatchRequestExceptions { get; set; }

        #region Api helper methods

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
        /// <param name="error">Enum indicating the error, .None if there is no error</param>
        /// <returns>True when the JSON could be parsed and it didn't describe a known Last.Fm error.</returns>
        public static bool IsResponseValid(string json, out LastFmApiError error)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                error = LastFmApiError.Unknown;
                return false;
            }

            JObject jo;
            try
            {
                jo = JsonConvert.DeserializeObject<JObject>(json);
            }
            catch (JsonException)
            {
                error = LastFmApiError.Unknown;
                return false;
            }

            var codeString = jo.Value<string>("error");
            if (string.IsNullOrWhiteSpace(codeString) && json.Length > 1)
            {
                error = LastFmApiError.None;
                return true;
            }

            error = LastFmApiError.Unknown;

            int code;
            if (Int32.TryParse(codeString, out code))
            {
                switch (code)
                {
                    case 2:
                        error = LastFmApiError.ServiceServiceWhereArtThou;
                        break;
                    case 3:
                        error = LastFmApiError.BadMethod;
                        break;
                    case 4:
                        error = LastFmApiError.BadAuth;
                        break;
                    case 5:
                        error = LastFmApiError.BadFormat;
                        break;
                    case 6:
                        error = LastFmApiError.MissingParameters;
                        break;
                    case 7:
                        error = LastFmApiError.BadResource;
                        break;
                    case 8:
                        error = LastFmApiError.Failure;
                        break;
                    case 9:
                        error = LastFmApiError.SessionExpired;
                        break;
                    case 10:
                        error = LastFmApiError.BadApiKey;
                        break;
                    case 11:
                        error = LastFmApiError.ServiceDown;
                        break;
                    case 13:
                        error = LastFmApiError.BadMethodSignature;
                        break;
                    case 16:
                        error = LastFmApiError.TemporaryFailure;
                        break;
                    case 26:
                        error = LastFmApiError.KeySuspended;
                        break;
                    case 29:
                        error = LastFmApiError.RateLimited;
                        break;
                }
            }

            return false;
        }

        #endregion
    }
}