using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class GetTrackInfoCommand : GetAsyncCommandBase<LastResponse<LastTrack>>
    {
        public string TrackMbid { get; set; }

        public string TrackName { get; set; }

        public string ArtistName { get; set; }

        public string Username { get; set; }

        public bool Autocorrect { get; set; }

        public GetTrackInfoCommand(ILastAuth auth)
            : base(auth)
        {
            Method = "track.getInfo";
        }

        public override void SetParameters()
        {
            if (TrackMbid != null)
            {
                Parameters.Add("mbid", TrackMbid);
            }
            else
            {
                Parameters.Add("track", TrackName);
                Parameters.Add("artist", ArtistName);
            }

            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            if (!string.IsNullOrWhiteSpace(Username))
            {
                Parameters.Add("username", Username);
            }
        }

        public async override Task<LastResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var track = LastTrack.ParseJToken(jtoken.SelectToken("track"));

                return LastResponse<LastTrack>.CreateSuccessResponse(track);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastTrack>>(error);
            }
        }
    }
}
