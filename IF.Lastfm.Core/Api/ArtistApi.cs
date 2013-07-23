using System;
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

        public async Task<LastResponse<Artist>> GetArtistInfoAsync(string artist,
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

        public async Task<PageResponse<Album>> GetTopAlbumsForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
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
    }
}