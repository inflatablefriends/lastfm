using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getShouts")]
    internal class GetShoutsCommand : GetAsyncCommandBase<PageResponse<LastShout>>
    {
        public string Username { get; set; }

        public GetShoutsCommand(ILastAuth auth, string username) : base(auth)
        {
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

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var shoutsToken = jtoken.SelectToken("shouts");
                var itemsToken = shoutsToken.SelectToken("shout");
                var pageInfoToken = jtoken.SelectToken("@attr");

                return PageResponse<LastShout>.CreateSuccessResponse(itemsToken, pageInfoToken, LastShout.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastShout>>(status);
            }
        }
    }
}