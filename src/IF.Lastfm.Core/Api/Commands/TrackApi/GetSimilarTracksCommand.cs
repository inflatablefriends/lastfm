using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class GetSimilarTracksCommand : GetAsyncCommandBase<LastResponse<List<LastTrack>>>
    {
        public string ArtistName { get; set; }

        public int? Limit { get; set; }

        public bool Autocorrect { get; set; }

        public string TrackName { get; set; }

        public GetSimilarTracksCommand(IAuth auth, string trackName, string artistName)
            : base(auth)
        {
            Method = "track.getSimilar";

            ArtistName = artistName;
            TrackName = trackName;
        }

        public override void SetParameters()
        {
            Parameters.Add("track", TrackName);
            Parameters.Add("artist", ArtistName);

            if (Limit != null)
            {
                Parameters.Add("limit", Limit.ToString());
            }

            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());
            
            DisableCaching();
        }

        public async override Task<LastResponse<List<LastTrack>>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var tracks = jtoken.SelectToken("similartracks")
                    .SelectToken("track")
                    .Children().Select(LastTrack.ParseJToken)
                    .ToList();

                return LastResponse<List<LastTrack>>.CreateSuccessResponse(tracks);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<List<LastTrack>>>(error);
            }
        }
    }
}
