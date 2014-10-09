using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class SearchTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string TrackName { get; set; }

        public SearchTracksCommand(IAuth auth, string trackName)
            : base(auth)
        {
            Method = "track.search";
            TrackName = trackName;
        }

        public override void SetParameters()
        {
            Parameters.Add("track", TrackName);

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var tracks = jtoken.SelectToken("results")
                    .SelectToken("trackmatches")
                    .SelectToken("track")
                    .Children().Select(LastTrack.ParseJToken)
                    .ToList();

                var pageresponse = PageResponse<LastTrack>.CreateSuccessResponse(tracks);

                var attrToken = jtoken.SelectToken("results");
                pageresponse.AddPageInfoFromOpenQueryJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(error);
            }
        }
    }
}
