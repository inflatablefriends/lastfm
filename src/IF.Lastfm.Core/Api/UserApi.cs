using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;
using IF.Lastfm.Core.Helpers;

namespace IF.Lastfm.Core.Api
{
    public class UserApi : ApiBase, IUserApi
    {


        public UserApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }

        public async Task<PageResponse<LastArtist>> GetRecommendedArtistsAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetRecommendedArtistsCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> GetTopAlbums(string username, LastStatsTimeSpan span, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTopAlbumsCommand(Auth, username, span)
            {
                Page = pagenumber,
                Count = count,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastArtist>> GetTopArtists(string username, LastStatsTimeSpan span, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTopArtistsCommand(Auth, username, span)
            {
                Page = pagenumber,
                Count = count,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetTopTracks(string username, LastStatsTimeSpan period = LastStatsTimeSpan.Week, int pageNumber = LastFm.DefaultPage, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTopTracksCommand(Auth, username)
            {
                Page = pageNumber,
                Count = count,
                Period = period,
		        HttpClient = HttpClient
	        };

	     return await command.ExecuteAsync();
        }

        /// <summary>
        /// Gets a list of recent scrobbled tracks for this user in reverse date order.
        /// </summary>
        /// <param name="username">Username to get scrobbles for.</param>
        /// <param name="from">Lower threshold for scrobbles. Will not return scrobbles from before this time.</param>
        /// <param name="to">Upper threshold for scrobbles. Will not return scrobbles from after this time.</param>
        /// <param name="pagenumber">Page numbering starts from 1. If set to 0, will not include the "now playing" track</param>
        /// <param name="extendedResponse">Determines if the response will contain extended data in each artist 
        /// and whether or not the user has loved each track</param>
        /// <param name="count">Amount of scrobbles to return for this page.</param>
        /// <returns>Enumerable of LastTrack</returns>
        public async Task<PageResponse<LastTrack>> GetRecentScrobbles(string username, DateTimeOffset? from = null, 
            DateTimeOffset? to = null, bool extendedResponse = false, int pagenumber = LastFm.DefaultPage,
            int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentTracksCommand(Auth, username)
            {
                Page = pagenumber,
                Count = count,
                From = from,
                To = to,
                Extended = extendedResponse,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastStation>> GetRecentStations(string username, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentStationsCommand(Auth, username)
            {
                Page = pagenumber,
                Count = count,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastShout>> GetShoutsAsync(string username, int pagenumber, int count = LastFm.DefaultPageLength)
        {
            var command = new GetShoutsCommand(Auth, username)
            {
                Page = pagenumber,
                Count = count,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastUser>> GetInfoAsync(string username)
        {
            var command = new GetInfoCommand(Auth, username)
            {
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> AddShoutAsync(string recipient, string message)
        {
            var command = new AddShoutCommand(Auth, recipient, message)
            {
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetLovedTracks(
            string username,
            int pagenumber = 1,
            int count = LastFm.DefaultPageLength)
        {
            var command = new GetLovedTracksCommand(auth: Auth, username: username)
                              {
                                  Page = pagenumber,
                                  Count = count,
                                  HttpClient = HttpClient
                              };
            return await command.ExecuteAsync();
        }
        public async Task<PageResponse<LastWeeklyChartList>> GetWeeklyChartListAsync(string username)
        {
            var command = new GetWeeklyChartListCommand(auth: Auth, username: username)
            {
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }
        
        public async Task<PageResponse<LastArtist>> GetWeeklyArtistChartAsync(string username, double? from = null, double? to = null)
        {
            var command = new GetWeeklyArtistChartCommand(auth: Auth, username: username)
            {
                From = from,
                To = to,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        } 

        public async Task<PageResponse<LastTrack>> GetWeeklyTrackChartAsync(string username, double? from = null, double? to = null)
        {
            var command = new GetWeeklyTrackChartCommand(auth: Auth, username: username)
            {
                From = from,
                To = to,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> GetWeeklyAlbumChartAsync(string username, double? from = null, double? to = null)
        {
            var command = new GetWeeklyAlbumChartCommand(auth: Auth, username: username)
            {
                From = from,
                To = to,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }
    }
}
