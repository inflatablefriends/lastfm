using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Album;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class AlbumApi : ApiBase, IAlbumApi
    {
        public AlbumApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }

        public async Task<LastResponse<LastAlbum>> GetInfoAsync(string artistname, string albumname, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth, albumname, artistname)
                          {
                              Autocorrect = autocorrect,
                              HttpClient = HttpClient
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastAlbum>> GetInfoByMbidAsync(string albumMbid, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth)
            {
                AlbumMbid = albumMbid,
                Autocorrect = autocorrect,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        //public Task<PageResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist, string album, CountryCode country, bool autocorrect = false)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<PageResponse<LastTag>> GetTagsByUserAsync(string artist, string album, string username, bool autocorrect = false)
        {
            var command = new GetTagsByUserCommand(Auth, artist, album, username)
            {
                Autocorrect = autocorrect,
                HttpClient = HttpClient
            };

            return command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTag>> GetTopTagsAsync(string artist, string album, bool autocorrect = false)
        {
            var command = new GetTopTagsCommand(Auth)
            {
                ArtistName = artist,
                AlbumName = album,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> SearchAsync(string albumname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchCommand(Auth, albumname)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastShout>> GetShoutsAsync(string albumname, string artistname, bool autocorrect = false, int page = 1, int count = LastFm.DefaultPageLength)
        {
            var command = new GetShoutsCommand(Auth, albumname, artistname)
            {
                Page = page,
                Autocorrect = autocorrect,
                Count = count,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        //public async Task<LastResponse> AddShoutAsync(string albumname, string artistname, string message)
        //{
        //    var command = new AddShoutCommand(Auth, albumname, artistname, message);

        //    return await command.ExecuteAsync();
        //}
    }
}