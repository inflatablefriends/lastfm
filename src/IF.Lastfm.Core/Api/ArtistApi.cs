using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.ArtistApi;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class ArtistApi : IArtistApi
    {
        public IAuth Auth { get; private set; }

        public ArtistApi(IAuth auth)
        {
            Auth = auth;
        }

        public async Task<LastResponse<LastArtist>> GetArtistInfoAsync(string artist,
            string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false)
        {
            var command = new GetArtistInfoCommand(Auth, artist)
            {
                BioLanguage = bioLang,
                Autocorrect = autocorrect
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> GetTopAlbumsForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetArtistTopAlbumsCommand(Auth, artist);
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetTopTracksForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetArtistTopTracksCommand(Auth, artist);
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<List<LastArtist>>> GetSimilarArtistsAsync(string artistname, bool autocorrect = false, int limit = 100)
        {
            var command = new GetSimilarArtistsCommand(Auth, artistname, autocorrect, limit);
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<Tag>> GetUserTagsForArtistAsync(string artist,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResponse<Tag>> GetTopTagsForArtistAsync(string artist, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResponse<Shout>> GetShoutsForArtistAsync(string artist, int page = 0, int count = LastFm.DefaultPageLength, bool autocorrect = false)
        {
            var command = new GetArtistShoutsCommand(Auth, artist)
                          {
                              Autocorrect = autocorrect,
                              Page = page,
                              Count = count
                          };
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> AddShoutAsync(string artistname, string messaage)
        {
            var command = new AddShoutCommand(Auth, artistname, messaage);

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastArtist>> SearchForArtistAsync(string artistname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchArtistsCommand(Auth, artistname)
            {
                Page = page,
                Count = itemsPerPage
            };

            return await command.ExecuteAsync();
        }
    }
}