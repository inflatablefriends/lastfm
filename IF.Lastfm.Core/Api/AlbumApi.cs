using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.AlbumApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api
{
    public class AlbumApi : IAlbumApi
    {
        public IAuth Auth { get; private set; }

        public AlbumApi(IAuth auth)
        {
            Auth = auth;
        }

        public async Task<LastResponse<Album>> GetAlbumInfoAsync(string artistname, string albumname, bool autocorrect = false)
        {
            var command = new GetAlbumInfoCommand(Auth, artistname, albumname)
                          {
                              Autocorrect = autocorrect
                          };

            return await command.ExecuteAsync();
        }

        public Task<PageResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist, string album, CountryCode country, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<PageResponse<Shout>> GetShoutsForAlbumAsync(string artist, string album, bool autocorrect = false, int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
        }

        public Task<PageResponse<Tag>> GetUserTagsForAlbumAsync(string artist, string album, string username, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<PageResponse<Tag>> GetTopTagsForAlbumAsync(string artist, string album, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<PageResponse<Album>> SearchForAlbumAsync(string album, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
        }
    }
}