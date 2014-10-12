using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.ChartApi
{
    internal class GetTopTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public GetTopTracksCommand(ILastAuth auth)
            : base(auth)
        {
            Method = "chart.getTopTracks";
        }

        public override void SetParameters()
        {
            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var tracksToken = jtoken.SelectToken("tracks");
                var itemsToken = tracksToken.SelectToken("track");
                var pageInfoToken = tracksToken.SelectToken("@attr");

                return PageResponse<LastTrack>.CreateSuccessResponse(itemsToken, pageInfoToken, LastTrack.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(error);
            }
        }
    }
}
