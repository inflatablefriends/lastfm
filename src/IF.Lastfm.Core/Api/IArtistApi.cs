using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IArtistApi
    {
        ILastAuth Auth { get; }

        Task<LastResponse<LastArtist>> GetArtistInfoAsync(string artist, string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false);

        Task<LastResponse<LastArtist>> GetArtistInfoByMbidAsync(string mbid, string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false);

        Task<PageResponse<LastArtist>> GetSimilarArtistsAsync(string artistname, bool autocorrect = false, int limit = 100);

        Task<PageResponse<LastAlbum>> GetTopAlbumsForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastTrack>> GetTopTracksForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastTag>> GetUserTagsForArtistAsync(string artist,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastTag>> GetTopTagsForArtistAsync(string artist, bool autocorrect = false);

        Task<PageResponse<LastShout>> GetShoutsForArtistAsync(string artistname,
            int page = 0,
            int count = LastFm.DefaultPageLength,
            bool autocorrect = false);

        Task<LastResponse> AddShoutAsync(string artistname, string message);

        Task<PageResponse<LastArtist>> SearchForArtistAsync(string artistname,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

    }
}
