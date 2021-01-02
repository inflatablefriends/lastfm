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
    [ApiMethodName("auth.getSession")]
    internal class GetSessionCommand : UnauthenticatedPostAsyncCommandBase<LastResponse<LastUserSession>>
    {
        private string Token { get; }

        public GetSessionCommand(ILastAuth auth, string authToken) : base(auth)
        {
            Token = authToken;
        }

        protected override Uri BuildRequestUrl()
        {
            return new Uri(LastFm.ApiRootSsl, UriKind.Absolute);
        }

        public override void SetParameters()
        {
            Parameters.Add("token", Token);
        }

        public override async Task<LastResponse<LastUserSession>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            if (LastFm.IsResponseValid(json, out LastResponseStatus status) && response.IsSuccessStatusCode)
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