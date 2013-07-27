using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IUserApi
    {
        IAuth Auth { get; }

        Task<PageResponse<Album>> GetTopAlbums(string username,
            LastStatsTimeSpan span,
            int startIndex = 0,
            int endIndex = LastFm.DefaultPageLength);

        Task<PageResponse<Track>> GetRecentScrobbles(string username,
            DateTime since,
            int startIndex = 0,
            int endIndex = LastFm.DefaultPageLength);

        Task<PageResponse<Station>> GetRecentStations(string username,
            int pagenumber,
            int count = LastFm.DefaultPageLength);

        Task<PageResponse<Shout>> GetShoutsAsync(string username,
            int pagenumber,
            int count = LastFm.DefaultPageLength);

        Task<LastResponse<User>> GetInfoAsync(string username);
    }
}