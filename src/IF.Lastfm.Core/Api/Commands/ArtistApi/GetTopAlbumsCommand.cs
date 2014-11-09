using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.ArtistApi
{
    internal class GetTopAlbumsCommand : GetAsyncCommandBase<PageResponse<LastAlbum>>
    {
        public string ArtistName { get; set; }

        public GetTopAlbumsCommand(ILastAuth auth, string artistname)
            : base(auth)
        {
            Method = "artist.getTopAlbums";
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
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var albumsToken = jtoken.SelectToken("topalbums");
                var itemsToken = albumsToken.SelectToken("album");
                var pageInfoToken = albumsToken.SelectToken("@attr");

                return PageResponse<LastAlbum>.CreateSuccessResponse(itemsToken, pageInfoToken, LastAlbum.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(error);
            }
        }
    }
}
