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

        Task<ListResponse<Album>> GetTopAlbumsForArtistAsync(string artist,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<ListResponse<Album>> GetTopAlbumsForArtistWithMbidAsync(string mbid,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        #endregion

        #region artist.getTags

        Task<ListResponse<Tag>> GetUserTagsForArtistAsync(string artist,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<ListResponse<Tag>> GetUserTagsForArtistWithMbidAsync(string mbid,
            string username,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        #endregion

        #region artist.getTopTags

        Task<ListResponse<Tag>> GetTopTagsForArtistAsync(string artist, bool autocorrect = false);

        Task<ListResponse<Tag>> GetTopTagsForArtistWithMbidAsync(string mbid, bool autocorrect = false);

        #endregion

    }
}
