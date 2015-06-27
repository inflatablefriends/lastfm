using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getTopArtists")]
    internal class GetTopArtistsCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public string Username { get; set; }
        public LastStatsTimeSpan TimeSpan { get; set; }

        public GetTopArtistsCommand(ILastAuth auth, string username, LastStatsTimeSpan span)
            : base(auth)
        {
            Username = username;
            TimeSpan = span;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            Parameters.Add("period", TimeSpan.GetApiName());

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var topArtistsToken = jtoken.SelectToken("topartists");
                var itemsToken = topArtistsToken.SelectToken("artist");
                var pageInfoToken = topArtistsToken.SelectToken("@attr");

                return PageResponse<LastArtist>.CreateSuccessResponse(itemsToken, pageInfoToken, LastArtist.ParseJToken, LastPageResultsType.Attr);
            }

            return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(status);
        }
    }
}
