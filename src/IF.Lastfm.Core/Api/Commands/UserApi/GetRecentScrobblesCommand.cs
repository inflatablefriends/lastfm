using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetRecentScrobblesCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string Username { get; private set; }

        public DateTime From { get; private set; }

        public GetRecentScrobblesCommand(ILastAuth auth, string username, DateTime from) : base(auth)
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
                



                IEnumerable<LastTrack> tracks;

                var tracksToken = jtoken.SelectToken("track");

               
                tracks = tracksToken.Type == JTokenType.Array
                    ? tracksToken.Children().Select(t => LastTrack.ParseJToken(t))
                    : new List<LastTrack>() { LastTrack.ParseJToken(tracksToken) };


                



                //var tracks = new List<LastTrack>();
                //foreach (var track in tracksToken.Children())
                //{
                //    var t = LastTrack.ParseJToken(track);
                    
                //    tracks.Add(t);
                //}

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