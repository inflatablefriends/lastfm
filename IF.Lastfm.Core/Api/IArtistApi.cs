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

        #region artist.getInfo

        Task<LastResponse<Artist>> GetArtistInfoAsync(string artist, string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false);

        Task<LastResponse<Artist>> GetArtistInfoWithMbidAsync(string mbid, string bioLang = LastFm.DefaultLanguageCode,
            bool autocorrect = false);

        #endregion

        #region artist.getTopAlbums

        Task<PageResponse<Album>> GetTopAlbumsForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<Album>> GetTopAlbumsForArtistWithMbidAsync(string mbid,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        #endregion

        #region artist.getTags

        Task<PageResponse<Tag>> GetUserTagsForArtistAsync(string artist,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<Tag>> GetUserTagsForArtistWithMbidAsync(string mbid,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        #endregion

        #region artist.getTopTags

        Task<PageResponse<Tag>> GetTopTagsForArtistAsync(string artist, bool autocorrect = false);

        Task<PageResponse<Tag>> GetTopTagsForArtistWithMbidAsync(string mbid, bool autocorrect = false);

        #endregion

    }
}
