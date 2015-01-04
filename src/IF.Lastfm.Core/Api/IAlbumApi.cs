using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IAlbumApi
    {
        ILastAuth Auth { get; }

        Task<LastResponse<LastAlbum>> GetAlbumInfoAsync(string artist, string album, bool autocorrect = false);

        Task<LastResponse<LastAlbum>> GetAlbumInfoByMbidAsync(string albumMbid, bool autocorrect = false);
        
        //Task<PageResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist,
        //    string album,
        //    CountryCode country,
        //    bool autocorrect = false);
        
        Task<PageResponse<LastTag>> GetTagsByUserAsync(string artist,
            string album,
            string username,
            bool autocorrect = false);

        Task<PageResponse<LastTag>> GetTopTagsForAlbumAsync(string artist,
            string album,
            bool autocorrect = false);

        Task<PageResponse<LastAlbum>> SearchForAlbumAsync(string album,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastShout>> GetShoutsAsync(string albumname,
            string artistname,
            bool autocorrect = false,
            int page = 1,
            int count = LastFm.DefaultPageLength);

        //Task<LastResponse> AddShoutAsync(string albumname, string artistname, string message);
    }
}