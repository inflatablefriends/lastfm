using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.AlbumApi;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class AlbumApi : IAlbumApi
    {
        public ILastAuth Auth { get; private set; }

        public AlbumApi(ILastAuth auth)
        {
            Auth = auth;
        }

        public async Task<LastResponse<LastAlbum>> GetAlbumInfoAsync(string artistname, string albumname, bool autocorrect = false)
        {
            var command = new GetAlbumInfoCommand(Auth, albumname, artistname)
                          {
                              Autocorrect = autocorrect
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastAlbum>> GetAlbumInfoByMbidAsync(string albumMbid, bool autocorrect = false)
        {
            var command = new GetAlbumInfoCommand(Auth)
            {
                AlbumMbid = albumMbid,
                Autocorrect = autocorrect
            };

            return await command.ExecuteAsync();
        }

        public Task<PageResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist, string album, CountryCode country, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<PageResponse<LastTag>> GetUserTagsForAlbumAsync(string artist, string album, string username, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<PageResponse<LastTag>> GetTopTagsForAlbumAsync(string artist, string album, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResponse<LastAlbum>> SearchForAlbumAsync(string albumname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchAlbumsCommand(Auth, albumname)
            {
                Page = page,
                Count = itemsPerPage
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<Shout>> GetShoutsAsync(string albumname, string artistname, bool autocorrect = false, int page = 1, int count = LastFm.DefaultPageLength)
        {
            var command = new GetAlbumShoutsCommand(Auth, albumname, artistname)
                          {
                              Page = page,
                              Autocorrect = autocorrect,
                              Count = count
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