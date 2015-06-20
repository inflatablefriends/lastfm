using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Track
{
    [ApiMethodName(LastMethodsNames.track_getSimilar)]
    internal class GetSimilarCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public override string Method
        { get { return LastMethodsNames.track_getSimilar; } }

        public string ArtistName { get; set; }

        public int? Limit { get; set; }

        public bool Autocorrect { get; set; }

        public string TrackName { get; set; }

        public GetSimilarCommand(ILastAuth auth, string trackName, string artistName)
            : base(auth)
        {
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

        public async override Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var itemsToken = jtoken.SelectToken("similartracks").SelectToken("track");

                return PageResponse<LastTrack>.CreateSuccessResponse(itemsToken, LastTrack.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(status);
            }
        }
    }
}
