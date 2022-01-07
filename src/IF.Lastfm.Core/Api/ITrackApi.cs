using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Scrobblers;

namespace IF.Lastfm.Core.Api
{
    public interface ITrackApi
    {
        ILastAuth Auth { get; }

        [Obsolete("This method has been moved to the Scrobbler class. More information can be found at " + LastFm.SCROBBLING_HELP_URL)]
        Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble);
        //Task<LastResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobble);

        Task<LastResponse> UpdateNowPlayingAsync(Scrobble scrobble);

        Task<PageResponse<LastShout>> GetShoutsForTrackAsync(string trackname,
            string artistname,
            bool autocorrect = false,
            int page = 0,
            int count = LastFm.DefaultPageLength);

        Task<LastResponse<LastTrack>> GetInfoAsync(string trackname, string artistname, string username = "");
        Task<LastResponse<LastTrack>> GetInfoByMbidAsync(string mbid);
        Task<PageResponse<LastTrack>> GetSimilarAsync(string trackname, string artistname, bool autocorrect = false, int limit = 100);

        Task<LastResponse> LoveAsync(string trackname, string artistname);
        Task<LastResponse> UnloveAsync(string trackname, string artistname);

        Task<PageResponse<LastTrack>> SearchAsync(string trackname,
           string artistname = "",
           int page = 1,
           int itemsPerPage = LastFm.DefaultPageLength);

        //Task<LastResponse> AddShoutAsync(string trackname, string artistname, string message);
    }
}