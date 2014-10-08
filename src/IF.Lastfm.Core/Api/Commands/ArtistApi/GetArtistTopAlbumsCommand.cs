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

namespace IF.Lastfm.Core.Api.Commands.ArtistApi
{
    internal class GetArtistTopAlbumsCommand : GetAsyncCommandBase<PageResponse<LastAlbum>>
    {
        public string ArtistName { get; set; }

        public GetArtistTopAlbumsCommand(IAuth auth, string artistname)
            : base(auth)
        {
            Method = "artist.topAlbums";
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);
            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastAlbum>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (!LastFm.IsResponseValid(json, out error) || !response.IsSuccessStatusCode)
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(error);

            var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("topalbums");
            return LastAlbum.ParsePageJToken(jtoken.SelectToken("album"), jtoken.SelectToken("@attr"));
        }
    }
}
