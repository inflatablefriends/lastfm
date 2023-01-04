using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IUserApi
    {
        ILastAuth Auth { get; }

        Task<PageResponse<LastArtist>> GetRecommendedArtistsAsync(
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastAlbum>> GetTopAlbums(string username,
            LastStatsTimeSpan span,
            int startIndex = 0,
            int endIndex = LastFm.DefaultPageLength);

        Task<PageResponse<LastArtist>> GetTopArtists(string username,
            LastStatsTimeSpan span,
            int pagenumber = 0,
            int count = LastFm.DefaultPageLength);

        Task<PageResponse<LastTag>> GetTopTags(string username,
	        int count = LastFm.DefaultPageLength);

        Task<PageResponse<LastTrack>> GetRecentScrobbles(string username, DateTimeOffset? from = null, 
            DateTimeOffset? to = null, bool extendedResponse = false, int pagenumber = LastFm.DefaultPage,
            int count = LastFm.DefaultPageLength);

        Task<PageResponse<LastTrack>> GetTopTracks(string username, LastStatsTimeSpan period = LastStatsTimeSpan.Week, int pagenumber = LastFm.DefaultPage,
	        int count = LastFm.DefaultPageLength);

        Task<PageResponse<LastStation>> GetRecentStations(string username,
            int pagenumber,
            int count = LastFm.DefaultPageLength);

        Task<PageResponse<LastShout>> GetShoutsAsync(string username,
            int pagenumber,
            int count = LastFm.DefaultPageLength);

        Task<LastResponse<LastUser>> GetInfoAsync(string username);

        Task<LastResponse> AddShoutAsync(string recipient, string message);

        Task<PageResponse<LastTrack>> GetLovedTracks(string username, int pagenumber, int count);
        
        Task<PageResponse<LastWeeklyChartList>> GetWeeklyChartListAsync(string username);
        
        Task<PageResponse<LastArtist>> GetWeeklyArtistChartAsync(string username, double? to = null, double? from = null);

        Task<PageResponse<LastTrack>> GetWeeklyTrackChartAsync(string username, double? to = null, double? from = null);

        Task<PageResponse<LastAlbum>> GetWeeklyAlbumChartAsync(string username, double? to = null, double? from = null);
    }
}
