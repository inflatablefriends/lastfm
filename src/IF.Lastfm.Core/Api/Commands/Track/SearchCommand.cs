using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Track
{
    [ApiMethodName("track.search")]
    internal class SearchCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string TrackName { get; set; }

        public string Artist { get; set; }

        public SearchCommand(ILastAuth auth, string trackName, string artist = null)
            : base(auth)
        {
            TrackName = trackName;
            Artist = artist;
        }

        public override void SetParameters()
        {
            Parameters.Add("track", TrackName);
            if (Artist != null) Parameters.Add("artist", Artist);

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
                var resultsToken = jtoken.SelectToken("results");
                var itemsToken = resultsToken.SelectToken("trackmatches").SelectToken("track");

                return PageResponse<LastTrack>.CreateSuccessResponse(itemsToken, resultsToken, LastTrack.ParseJToken, LastPageResultsType.OpenQuery);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(status);
            }
        }
    }
}
