using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;

namespace IF.Lastfm.Core.Api
{
    public class UserApi : IUserApi
    {
        public ILastAuth Auth { get; private set; }

        public UserApi(ILastAuth auth)
        {
            Auth = auth;
        }

        public async Task<PageResponse<LastArtist>> GetRecommendedArtistsAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetRecommendedArtistsCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> GetTopAlbums(string username, LastStatsTimeSpan span, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTopAlbumsCommand(Auth, username, span)
                          {
                              Page = pagenumber,
                              Count = count
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
        public async Task<PageResponse<LastTrack>> GetRecentScrobbles(string username, DateTimeOffset? since = null, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentTracksCommand(Auth, username)
                          {
                              Page = pagenumber,
                              Count = count,
                              From = since
                          };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastStation>> GetRecentStations(string username, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentStationsCommand(Auth, username)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }
         
        public async Task<PageResponse<LastShout>> GetShoutsAsync(string username, int pagenumber, int count = LastFm.DefaultPageLength)
        {
            var command = new GetShoutsCommand(Auth, username)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastUser>> GetInfoAsync(string username)
        {
            var command = new GetInfoCommand(Auth, username);

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> AddShoutAsync(string recipient, string message)
        {
            var command = new AddShoutCommand(Auth, recipient, message);

            return await command.ExecuteAsync();
        }
    }
}