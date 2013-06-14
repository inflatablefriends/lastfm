using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api
{
    public class TrackApi : ITrackApi
    {
        public IAuth Auth { get; private set; }

        public TrackApi(IAuth auth)
        {
            Auth = auth;
        }

        public async Task<LastResponse> ScrobbleAsync(Scrobble scrobble)
        {
            const string apiMethod = "track.scrobble";

            var methodParameters = new Dictionary<string, string>
            {
                {"artist", scrobble.Artist},
                {"album", scrobble.Album},
                {"track", scrobble.Track},
                {"albumArtist", scrobble.AlbumArtist},
                {"chosenByUser", scrobble.ChosenByUser.ToInt().ToString()},
                {"timestamp", scrobble.TimePlayed.ToUnixTimestamp().ToString()},
                {"sk", Auth.User.Token}
            };

            var apisig = Auth.GenerateMethodSignature(apiMethod, methodParameters);

            var postContent = LastFm.CreatePostBody(apiMethod,
                Auth.ApiKey,
                apisig,
                methodParameters);

            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync(LastFm.ApiRoot, postContent);
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse(error);
            }
        }

        public Task<LastResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobble)
        {
            throw new NotImplementedException();
        }
    }
}