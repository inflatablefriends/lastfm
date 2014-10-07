using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IAlbumApi
    {
        IAuth Auth { get; }

        Task<LastResponse<FmAlbum>> GetAlbumInfoAsync(string artist, string album, bool autocorrect = false);
        
        Task<PageResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist,
            string album,
            CountryCode country,
            bool autocorrect = false);
        
        Task<PageResponse<Tag>> GetUserTagsForAlbumAsync(string artist,
            string album,
            string username,
            bool autocorrect = false);

        Task<PageResponse<Tag>> GetTopTagsForAlbumAsync(string artist,
            string album,
            bool autocorrect = false);

        Task<PageResponse<FmAlbum>> SearchForAlbumAsync(string album,
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<Shout>> GetShoutsAsync(string albumname,
            string artistname,
            bool autocorrect = false,
            int page = 1,
            int count = LastFm.DefaultPageLength);

        //Task<LastResponse> AddShoutAsync(string albumname, string artistname, string message);
    }
}