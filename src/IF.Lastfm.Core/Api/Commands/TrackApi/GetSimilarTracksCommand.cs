using System.Collections.Generic;
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
    internal class GetSimilarTracksCommand : GetAsyncCommandBase<LastResponse<List<LastTrack>>>
    {
        public string ArtistName;
        public int Limit;
        public bool Autocorrect { get; set; }
        public string TrackName { get; set; }

        public GetSimilarTracksCommand(IAuth auth, string trackName, string artistName, bool autocorrect, int limit)
            : base(auth)
        {
            ArtistName = artistName;
            Limit = limit;
            Autocorrect = autocorrect;
            Method = "track.getSimilar";
            TrackName = trackName;
        }

        public override void SetParameters()
        {
            Parameters.Add("track", TrackName);
            Parameters.Add("artist", ArtistName);
            Parameters.Add("limit", Limit.ToString());
            Parameters.Add("autocorrect", Autocorrect.ToInt().ToString());
            DisableCaching();
        }

        public async override Task<LastResponse<List<LastTrack>>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var tracks = jtoken.SelectToken("similartracks")
                    .SelectToken("track")
                    .Children().Select(LastTrack.ParseJToken)
                    .ToList();

                var lastresponse = LastResponse<List<LastTrack>>.CreateSuccessResponse(tracks);
                return lastresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<List<LastTrack>>>(error);
            }
        }
    }
}
