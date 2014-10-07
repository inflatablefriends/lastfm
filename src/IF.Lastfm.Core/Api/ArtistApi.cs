using System;
using System.Threading;
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

        public async Task<LastResponse<FmArtist>> GetArtistInfoAsync(string artist,
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

        public async Task<PageResponse<FmAlbum>> GetTopAlbumsForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetArtistTopAlbumsCommand(Auth, artist);
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<FmTrack>> GetTopTracksForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetArtistTopTracksCommand(Auth, artist);
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
    }
}