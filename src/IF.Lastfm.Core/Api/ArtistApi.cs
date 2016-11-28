using System.Net.Http;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Artist;
using IF.Lastfm.Core.Helpers;

namespace IF.Lastfm.Core.Api
{
    public class ArtistApi : ApiBase, IArtistApi
    {
        public ArtistApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }



        public async Task<LastResponse<LastArtist>> GetInfoAsync(string artist, string bioLang = LastFm.DefaultLanguageCode, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth)
            {
                ArtistName = artist,
                BioLanguage = bioLang,
                Autocorrect = autocorrect,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastArtist>> GetInfoByMbidAsync(string mbid, string bioLang = LastFm.DefaultLanguageCode, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth)
            {
                ArtistMbid = mbid,
                BioLanguage = bioLang,
                Autocorrect = autocorrect,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> GetTopAlbumsAsync(string artist, bool autocorrect = false, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopAlbumsCommand(Auth, artist)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetTopTracksAsync(string artist, bool autocorrect = false, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopTracksCommand(Auth, artist)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastArtist>> GetSimilarAsync(string artistname, bool autocorrect = false, int limit = LastFm.DefaultPageLength)
        {
            var command = new GetSimilarCommand(Auth)
            {
                ArtistName = artistname,
                Autocorrect = autocorrect,
                Limit = limit,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastArtist>> GetSimilarByMbidAsync(string mbid, bool autocorrect = false, int limit = LastFm.DefaultPageLength)
        {
            var command = new GetSimilarCommand(Auth)
            {
                ArtistMbid = mbid,
                Autocorrect = autocorrect,
                Limit = limit,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public Task<PageResponse<LastTag>> GetTagsByUserAsync(string artist, string username, bool autocorrect = false, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTagsByUserCommand(Auth, artist, username)
            {
                Autocorrect = autocorrect,
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };

            return command.ExecuteAsync();
        }

        public Task<PageResponse<LastTag>> GetTopTagsAsync(string artist, bool autocorrect = false)
        {
            var command = new GetTopTagsCommand(Auth)
            {
                ArtistName = artist,
                Autocorrect = autocorrect,
                HttpClient = HttpClient
            };

            return command.ExecuteAsync();
        }

        public Task<PageResponse<LastTag>> GetTopTagsByMbidAsync(string mbid, bool autocorrect = false)
        {
            var command = new GetTopTagsCommand(Auth)
            {
                ArtistMbid = mbid,
                Autocorrect = autocorrect,
                HttpClient = HttpClient
            };

            return command.ExecuteAsync();
        }


        public async Task<PageResponse<LastShout>> GetShoutsAsync(string artist, int page = 0, int count = LastFm.DefaultPageLength, bool autocorrect = false)
        {
            var command = new GetShoutsCommand(Auth, artist)
            {
                Autocorrect = autocorrect,
                Page = page,
                Count = count,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> AddShoutAsync(string artistname, string message)
        {
            var command = new AddShoutCommand(Auth, artistname, message)
            {
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastArtist>> SearchAsync(string artistname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchCommand(Auth, artistname)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }
    }
}