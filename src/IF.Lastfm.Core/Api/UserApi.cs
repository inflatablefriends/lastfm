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

        /// <summary>
        /// Gets a list of recent scrobbled tracks for this user in reverse date order.
        /// </summary>
        /// <param name="username">Username to get scrobbles for.</param>
        /// <param name="since">Lower threshold for scrobbles. Will not return scrobbles from before this time.</param>
        /// <param name="pagenumber">Page numbering starts from 1. If set to 0, will not include the "now playing" track</param>
        /// <param name="count">Amount of scrobbles to return for this page.</param>
        /// <returns>Enumerable of LastTrack</returns>
        public async Task<PageResponse<LastTrack>> GetRecentScrobbles(string username, DateTimeOffset? since = null, int pagenumber = 1, int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentTracksCommand(Auth, username)
            {
                Page = pagenumber,
                Count = count,
                From = since,
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
    }
}