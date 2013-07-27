using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetUserInfoCommand : GetAsyncCommandBase<LastResponse<User>>
    {
        public string Username { get; set; }

        public GetUserInfoCommand(IAuth auth, string username) : base(auth)
        {
            Method = "user.getInfo";
            Username = username;
        }

        public override Uri BuildRequestUrl()
        {
            var parameters = new Dictionary<string, string>
                             {
                                 {"user", Uri.EscapeDataString(Username)}
                             };

            base.DisableCaching(parameters);

            var uristring = LastFm.FormatApiUrl(Method, Auth.ApiKey, parameters);
            return new Uri(uristring, UriKind.Absolute);
        }

        public async override Task<LastResponse<User>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                return LastResponse<User>.CreateSuccessResponse(User.ParseJToken(jtoken.SelectToken("user")));
            }
            else
            {
                return LastResponse<User>.CreateErrorResponse(error);
            }
        }
    }
}