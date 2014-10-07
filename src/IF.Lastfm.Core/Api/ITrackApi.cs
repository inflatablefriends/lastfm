using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface ITrackApi
    {
        IAuth Auth { get; }

        Task<LastResponse> ScrobbleAsync(Scrobble scrobble);
        //Task<LastResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobble);

        Task<PageResponse<Shout>> GetShoutsForTrackAsync(string trackname,
            string artistname,
            bool autocorrect = false,
            int page = 0,
            int count = LastFm.DefaultPageLength);

        Task<LastResponse<LastTrack>> GetInfoAsync(string trackname, string artistname, string username = "");
        Task<LastResponse<LastTrack>> GetInfoByMbidAsync(string mbid);
        Task<LastResponse<List<LastTrack>>> GetSimilarTracksAsync(string trackname, string artistname, bool autocorrect = false, int limit = 100);

        Task<LastResponse> LoveTrackAsync(string trackname, string artistname);
        Task<LastResponse> UnloveTrackAsync(string trackname, string artistname);

        Task<PageResponse<LastTrack>> SearchForTrackAsync(string trackname,
           int page = 1,
           int itemsPerPage = LastFm.DefaultPageLength);

        //Task<LastResponse> AddShoutAsync(string trackname, string artistname, string message);
    }
}