using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Album
{
    [ApiMethodName(LastMethodsNames.album_search)]
    internal class SearchCommand : GetAsyncCommandBase<PageResponse<LastAlbum>>
    {
        public override string Method
        { get { return LastMethodsNames.album_search; } }

        public string AlbumName { get; set; }

        public SearchCommand(ILastAuth auth, string albumName)
            : base(auth)
        {
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

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var resultsToken = jtoken.SelectToken("results");
                var itemsToken = resultsToken.SelectToken("albummatches").SelectToken("album");

                return PageResponse<LastAlbum>.CreateSuccessResponse(itemsToken, resultsToken, LastAlbum.ParseJToken, LastPageResultsType.OpenQuery);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(status);
            }
        }
    }
}
