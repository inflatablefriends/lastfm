using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IAlbumApi
    {
        IAuth Auth { get; }

        #region album.getInfo

        Task<LastResponse<Album>> GetAlbumInfoAsync(string artist, string album, bool autocorrect = false);
        Task<LastResponse<Album>> GetAlbumInfoWithMbidAsync(string mbid, bool autocorrect = false);
        
        #endregion

        #region album.getBuylinks

        Task<ListResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist,
            string album,
            CountryCode country,
            bool autocorrect = false);

        Task<ListResponse<BuyLink>> GetBuyLinksForAlbumWithMbidAsync(string mbid,
            CountryCode country,
            bool autocorrect = false);

        #endregion

        #region album.getShouts

        Task<ListResponse<Shout>> GetShoutsForAlbumAsync(string artist,
            string album,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<ListResponse<Shout>> GetShoutsForAlbumWithMbidAsync(string mbid,
            bool autocorrect = false,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        #endregion

        #region album.getTags

        Task<ListResponse<Tag>> GetUserTagsForAlbumAsync(string artist, string album, string username, bool autocorrect = false);
        Task<ListResponse<Tag>> GetUserTagsForAlbumWithMbidAsync(string mbid, string username, bool autocorrect = false);

        #endregion

        #region album.getTopTags

        Task<ListResponse<Tag>> GetTopTagsForAlbumAsync(string artist, string album, bool autocorrect = false);
        Task<ListResponse<Tag>> GetTopTagsForAlbumWithMbidAsync(string mbid, bool autocorrect = false);

        #endregion

        #region album.search

        Task<ListResponse<Album>> SearchForAlbumAsync(string album, int page = 1, int itemsPerPage = LastFm.DefaultPageLength);

        #endregion
    }
}