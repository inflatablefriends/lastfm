using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.AlbumApi
{
    internal class GetAlbumShoutsCommand : GetAsyncCommandBase<PageResponse<Shout>>
    {
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public bool Autocorrect { get; set; }

        public GetAlbumShoutsCommand(IAuth auth, string albumname, string artistname)
            : base(auth)
        {
            Method = "album.getShouts";
            AlbumName = albumname;
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("album", AlbumName);
            Parameters.Add("artist", ArtistName);
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            base.AddPagingParameters();
            base.DisableCaching();
        }
        
        public async override Task<PageResponse<Shout>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("shouts");

                return Shout.ParsePageJToken(jtoken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<Shout>>(error);
            }
        }
    }
}