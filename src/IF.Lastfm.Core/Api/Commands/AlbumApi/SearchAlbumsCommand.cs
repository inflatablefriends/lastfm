using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var resultsToken = jtoken.SelectToken("results");
                var itemsToken = resultsToken.SelectToken("albummatches").SelectToken("album");

                return PageResponse<LastAlbum>.CreateSuccessResponse(itemsToken, resultsToken, LastAlbum.ParseJToken, true);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(error);
            }
        }
    }
}
