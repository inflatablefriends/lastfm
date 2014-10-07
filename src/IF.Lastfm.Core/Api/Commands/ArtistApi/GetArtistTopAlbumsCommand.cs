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
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                //first, let's see how many albums
                //stupid api returns an object intead of an array when there is only one item
                var attrToken = jtoken.SelectToken("topalbums").SelectToken("@attr");

                var albums = new List<LastAlbum>();

                if (attrToken.Value<int>("count") > 1)
                    albums = jtoken.SelectToken("topalbums")
                        .SelectToken("album")
                        .Children().Select(LastAlbum.ParseJToken)
                        .ToList();
                else
                    albums.Add(LastAlbum.ParseJToken(jtoken.SelectToken("topalbums")
                        .SelectToken("album")));

                var pageresponse = PageResponse<LastAlbum>.CreateSuccessResponse(albums);

                
                pageresponse.AddPageInfoFromJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(error);
            }
        }
    }
}
