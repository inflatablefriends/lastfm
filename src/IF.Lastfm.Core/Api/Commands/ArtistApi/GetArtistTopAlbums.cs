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
    internal class GetArtistTopAlbumsCommand : GetAsyncCommandBase<PageResponse<Album>>
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

            base.DisableCaching();
        }

        public async override Task<PageResponse<Album>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var albums = new List<Album>();

                foreach (var jToken in jtoken.SelectToken("topalbums").SelectToken("album").Children())
                {
                    var album = Album.ParseJToken(jToken);
                    albums.Add(album);
                }

                var pageresponse = PageResponse<Album>.CreateSuccessResponse(albums);

                var attrToken = jtoken.SelectToken("topalbums").SelectToken("@attr");
                pageresponse.AddPageInfoFromJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<Album>>(error);
            }
        }
    }
}
