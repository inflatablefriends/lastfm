using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IArtistApi
    {
        ILastAuth Auth { get; }

        Task<LastResponse<LastArtist>> GetInfoAsync(string artist, string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false,
            string username = null);

        Task<LastResponse<LastArtist>> GetInfoByMbidAsync(string mbid, string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false,
            string username = null);

        Task<PageResponse<LastArtist>> GetSimilarAsync(string artistname, bool autocorrect = false, int limit = 100);

        Task<PageResponse<LastArtist>> GetSimilarByMbidAsync(string mbid, bool autocorrect = false, int limit = 100);

        Task<PageResponse<LastAlbum>> GetTopAlbumsAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastAlbum>> GetTopAlbumsByMbidAsync(string mbid,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);


        Task<PageResponse<LastTrack>> GetTopTracksAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastTag>> GetTagsByUserAsync(string artist,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastTag>> GetTopTagsAsync(string artist, bool autocorrect = false);

        Task<PageResponse<LastShout>> GetShoutsAsync(string artistname,
            int page = 0,
            int count = LastFm.DefaultPageLength,
            bool autocorrect = false);

        Task<LastResponse> AddShoutAsync(string artistname, string message);

        Task<PageResponse<LastArtist>> SearchAsync(string artistname,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

    }
}
