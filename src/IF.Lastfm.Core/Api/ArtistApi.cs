using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Artist;

namespace IF.Lastfm.Core.Api
{
    public class ArtistApi : IArtistApi
    {
        public ArtistApi(ILastAuth auth)
        {
            Auth = auth;
        }

        public ILastAuth Auth { get; private set; }

        public async Task<LastResponse<LastArtist>> GetArtistInfoAsync(string artist, string bioLang = LastFm.DefaultLanguageCode, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth)
            {
                ArtistName = artist,
                BioLanguage = bioLang,
                Autocorrect = autocorrect
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastArtist>> GetArtistInfoByMbidAsync(string mbid, string bioLang = LastFm.DefaultLanguageCode, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth)
            {
                ArtistMbid = mbid,
                BioLanguage = bioLang,
                Autocorrect = autocorrect
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> GetTopAlbumsForArtistAsync(string artist, bool autocorrect = false, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopAlbumsCommand(Auth, artist)
            {
                Page = page,
                Count = itemsPerPage
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetTopTracksForArtistAsync(string artist, bool autocorrect = false, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopTracksCommand(Auth, artist)
            {
                Page = page,
                Count = itemsPerPage
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastArtist>> GetSimilarArtistsAsync(string artistname, bool autocorrect = false, int limit = LastFm.DefaultPageLength)
        {
            var command = new GetSimilarCommand(Auth, artistname)
            {
                Autocorrect = autocorrect,
                Limit = limit
            };
            return await command.ExecuteAsync();
        }

        public Task<PageResponse<LastTag>> GetUserTagsForArtistAsync(string artist, string username, bool autocorrect = false, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTagsByUserCommand(Auth, artist, username)
            {
                Autocorrect = autocorrect,
                Page = page,
                Count = itemsPerPage
            };

            return command.ExecuteAsync();
        }

        public Task<PageResponse<LastTag>> GetTopTagsForArtistAsync(string artist, bool autocorrect = false)
        {
            var command = new GetTopTagsCommand(Auth, artist)
            {
                Autocorrect = autocorrect
            };

            return command.ExecuteAsync();
        }

        public async Task<PageResponse<LastShout>> GetShoutsForArtistAsync(string artist, int page = 0, int count = LastFm.DefaultPageLength, bool autocorrect = false)
        {
            var command = new GetShoutsCommand(Auth, artist)
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
            var command = new SearchCommand(Auth, artistname)
            {
                Page = page,
                Count = itemsPerPage
            };

            return await command.ExecuteAsync();
        }
    }
}