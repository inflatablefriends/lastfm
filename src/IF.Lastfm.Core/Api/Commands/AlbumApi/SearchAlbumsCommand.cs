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
    internal class SearchAlbumsCommand : GetAsyncCommandBase<PageResponse<LastAlbum>>
    {
        public string AlbumName { get; set; }

        public SearchAlbumsCommand(IAuth auth, string albumName)
            : base(auth)
        {
            Method = "album.search";
            AlbumName = albumName;
        }

        public override void SetParameters()
        {
            Parameters.Add("album", AlbumName);

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

                var albums = jtoken.SelectToken("results")
                    .SelectToken("albummatches")
                    .SelectToken("album")
                    .Children().Select(LastAlbum.ParseJToken)
                    .ToList();

                var pageresponse = PageResponse<LastAlbum>.CreateSuccessResponse(albums);

                var attrToken = jtoken.SelectToken("results");
                pageresponse.AddPageInfoFromOpenQueryJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(error);
            }
        }
    }
}
