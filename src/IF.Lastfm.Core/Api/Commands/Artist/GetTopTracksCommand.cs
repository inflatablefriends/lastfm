using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Artist
{
    [ApiMethodName("artist.getTopTracks")]
    internal class GetTopTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string ArtistName { get; set; }

        public GetTopTracksCommand(ILastAuth auth, string artistname)
            : base(auth)
        {
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);

            AddPagingParameters();
            DisableCaching();
        }

        public override async Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var tracksToken = jtoken.SelectToken("toptracks");
                var itemsToken = tracksToken.SelectToken("track");
                var pageInfoToken = tracksToken.SelectToken("@attr");

                return PageResponse<LastTrack>.CreateSuccessResponse(itemsToken, pageInfoToken, LastTrack.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(status);
            }
        }
    }
}
