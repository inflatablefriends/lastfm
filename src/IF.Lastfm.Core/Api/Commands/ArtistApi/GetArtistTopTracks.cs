using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.ArtistApi
{
    internal class GetArtistTopTracksCommand : GetAsyncCommandBase<PageResponse<FmTrack>>
    {
        public string ArtistName { get; set; }

        public GetArtistTopTracksCommand(IAuth auth, string artistname)
            : base(auth)
        {
            Method = "artist.topTracks";
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);

            base.DisableCaching();
        }

        public async override Task<PageResponse<FmTrack>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var tracks = new List<FmTrack>();

                foreach (var jToken in jtoken.SelectToken("toptracks").SelectToken("track").Children())
                {
                    var t = FmTrack.ParseJToken(jToken);
                    tracks.Add(t);
                }

                var pageresponse = PageResponse<FmTrack>.CreateSuccessResponse(tracks);

                var attrToken = jtoken.SelectToken("toptracks").SelectToken("@attr");
                pageresponse.AddPageInfoFromJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<FmTrack>>(error);
            }
        }
    }
}
