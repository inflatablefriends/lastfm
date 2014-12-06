using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetUserShoutsCommand : GetAsyncCommandBase<PageResponse<LastShout>>
    {
        public string Username { get; set; }

        public GetUserShoutsCommand(ILastAuth auth, string username) : base(auth)
        {
            Method = "user.getShouts";
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastShout>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var shoutsToken = jtoken.SelectToken("shouts");
                var itemsToken = shoutsToken.SelectToken("shout");
                var pageInfoToken = jtoken.SelectToken("@attr");

                return PageResponse<LastShout>.CreateSuccessResponse(itemsToken, pageInfoToken, LastShout.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastShout>>(error);
            }
        }
    }
}