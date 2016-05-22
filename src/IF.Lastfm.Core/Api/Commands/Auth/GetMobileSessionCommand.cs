using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Auth
{
    [ApiMethodName("auth.getMobileSession")]
    internal class GetMobileSessionCommand : UnauthenticatedPostAsyncCommandBase<LastResponse<LastUserSession>>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public GetMobileSessionCommand(ILastAuth auth, string username, string password) : base(auth)
        {
            Username = username;
            Password = password;
        }

        protected override Uri BuildRequestUrl()
        {
            return new Uri(LastFm.ApiRootSsl, UriKind.Absolute);
        }

        public override void SetParameters()
        {
            Parameters.Add("username", Username);
            Parameters.Add("password", Password);
        }

        public override async Task<LastResponse<LastUserSession>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var sessionObject = JsonConvert.DeserializeObject<JObject>(json).GetValue("session");
                var session = JsonConvert.DeserializeObject<LastUserSession>(sessionObject.ToString());

                return LastResponse<LastUserSession>.CreateSuccessResponse(session);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastUserSession>>(status);
            }
        }
    }
}
