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
        IAuth Auth { get; }

        Task<LastResponse<FmArtist>> GetArtistInfoAsync(string artist, string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false);

        Task<PageResponse<FmAlbum>> GetTopAlbumsForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<FmTrack>> GetTopTracksForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<Tag>> GetUserTagsForArtistAsync(string artist,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<Tag>> GetTopTagsForArtistAsync(string artist, bool autocorrect = false);

        Task<PageResponse<Shout>> GetShoutsForArtistAsync(string artistname,
            int page = 0,
            int count = LastFm.DefaultPageLength,
            bool autocorrect = false);

        Task<LastResponse> AddShoutAsync(string artistname, string messaage);
    }
}
