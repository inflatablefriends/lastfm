using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Auth
{
    [ApiMethodName(LastMethodsNames.auth_getMobileSession)]
    internal class GetMobileSessionCommand : UnauthenticatedPostAsyncCommandBase<LastResponse<LastUserSession>>
    {
        public override string Method
        { get { return LastMethodsNames.auth_getMobileSession; } }

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

        public async override Task<LastResponse<LastUserSession>> HandleResponse(HttpResponseMessage response)
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
