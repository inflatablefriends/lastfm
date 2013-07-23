using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class GetTrackInfoCommand : GetAsyncCommandBase<LastResponse<Track>>
    {
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
        public string Username { get; set; }
        public bool Autocorrect { get; set; }

        public GetTrackInfoCommand(IAuth auth, string trackname, string artistname)
            : base(auth)
        {
            Method = "track.getInfo";
            TrackName = trackname;
            ArtistName = artistname;
        }

        public async override Task<LastResponse<Track>> ExecuteAsync()
        {
            var parameters = new Dictionary<string, string>
                {
                    {"track", TrackName},
                    {"artist", ArtistName},
                    {"autocorrect", Convert.ToInt32(Autocorrect).ToString()}
                };

            if (!string.IsNullOrWhiteSpace(Username))
            {
                parameters.Add("username", Username);
            }

            var apiUrl = LastFm.FormatApiUrl(Method, Auth.ApiKey, parameters);
            Url = new Uri(apiUrl, UriKind.Absolute);

            return await ExecuteInternal();
        }

        public async override Task<LastResponse<Track>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var track = Track.ParseJToken(jtoken.SelectToken("track"));

                return LastResponse<Track>.CreateSuccessResponse(track);
            }
            else
            {
                return LastResponse<Track>.CreateErrorResponse(error);
            }
        }
    }
}
