using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Auth
{
    [ApiMethodName("auth.getToken")]
    internal class GetTokenCommand : UnauthenticatedPostAsyncCommandBase<LastResponse<string>>
    {
        public GetTokenCommand(ILastAuth auth) : base(auth)
        {
        }

        protected override Uri BuildRequestUrl()
        {
            return new Uri(LastFm.ApiRootSsl, UriKind.Absolute);
        }

        public override void SetParameters()
        {
        }

        public override async Task<LastResponse<string>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            if (LastFm.IsResponseValid(json, out LastResponseStatus status) && response.IsSuccessStatusCode)
            {
                var token = JsonConvert.DeserializeObject<JObject>(json).GetValue("token");
                return LastResponse<string>.CreateSuccessResponse(token.Value<string>());
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<string>>(status);
            }
        }
    }
}