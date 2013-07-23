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

        Task<PageResponse<Shout>> GetShoutsForTrackAsync(string trackname, string artistname,
                                              int page = 0,
                                              int count = LastFm.DefaultPageLength);

        Task<PageResponse<Shout>> GetShoutsForTrackWithMbidAsync(string mbid,
                                              int page = 0,
                                              int count = LastFm.DefaultPageLength);

        Task<LastResponse<Track>> GetInfoAsync(string trackname, string artistname, string username = "");
        Task<LastResponse<Track>> GetInfoWithMbidAsynnc(string mbid, string username = "");
    }
}