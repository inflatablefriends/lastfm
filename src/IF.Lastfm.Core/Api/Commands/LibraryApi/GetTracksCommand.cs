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

namespace IF.Lastfm.Core.Api.Commands.LibraryApi
{
    internal class GetTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string Username { get; private set; }

        public string Artist { get; private set; }

        public string Album { get; private set; }

        public DateTime From { get; private set; }

        public GetTracksCommand(ILastAuth auth, string username, string artist, string album, DateTime from) : base(auth)
        {
            Method = "library.getTracks";

            Username = username;
            Artist = artist;
            Album = album;
            From = from;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            Parameters.Add("artist", Artist);
            Parameters.Add("album", Album);
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
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("tracks");

                var tracksToken = jtoken.SelectToken("track");

                var pageInfoToken = jtoken.SelectToken("@attr");
 
                return PageResponse<LastTrack>.CreateSuccessResponse(tracksToken, pageInfoToken, LastTrack.ParseJToken, false);

            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(error);
            }
        }
    }
}