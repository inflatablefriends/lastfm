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
        Task<LastResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobble);

        Task<PageResponse<Shout>> GetShoutsForTrackAsync(string trackname,
            string artistname,
            bool autocorrect = false,
            int page = 0,
            int count = LastFm.DefaultPageLength);

        Task<LastResponse<Track>> GetInfoAsync(string trackname, string artistname, string username = "");

        Task<LastResponse> LoveTrackAsync(string trackname, string artistname);
        Task<LastResponse> UnloveTrackAsync(string trackname, string artistname);

        //Task<LastResponse> AddShoutAsync(string trackname, string artistname, string message);
    }
}