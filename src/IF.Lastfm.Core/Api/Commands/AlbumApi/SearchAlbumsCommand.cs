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
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (!LastFm.IsResponseValid(json, out error) || !response.IsSuccessStatusCode)
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(error);

            var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("results");
            return PageResponse<LastAlbum>.CreatePageResponse(jtoken.SelectToken("albumsmatches").SelectToken("album"),
                jtoken, LastAlbum.ParseJToken, true);
        }
    }
}
