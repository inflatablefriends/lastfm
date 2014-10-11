using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetRecentScrobblesCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string Username { get; private set; }

        public DateTime From { get; private set; }

        public GetRecentScrobblesCommand(IAuth auth, string username, DateTime from) : base(auth)
        {
            Method = "user.getRecentTracks";

            Username = username;
            From = from;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            Parameters.Add("from", From.ToUnixTimestamp().ToString());

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("recenttracks");

                var tracksToken = jtoken.SelectToken("track");

                var tracks = new List<LastTrack>();
                foreach (var track in tracksToken.Children())
                {
                    var t = LastTrack.ParseJToken(track);
                    
                    tracks.Add(t);
                }

                var pageresponse = PageResponse<LastTrack>.CreateSuccessResponse(tracks);

                var attrToken = jtoken.SelectToken("@attr");
                pageresponse.AddPageInfoFromJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(error);
            }
        }
    }
}