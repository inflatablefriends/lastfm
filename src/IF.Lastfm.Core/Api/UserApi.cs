using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.UserApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class UserApi : IUserApi
    {
        public IAuth Auth { get; private set; }

        public UserApi(IAuth auth)
        {
            Auth = auth;
        }

        /// <summary>
        /// Gets the top albums for the given user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="span"></param>
        /// <param name="pagenumber"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PageResponse<FmAlbum>> GetTopAlbums(string username, LastStatsTimeSpan span, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTopAlbumsCommand(Auth, username, span)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        /// <summary>
        /// Gets scrobbles and stuff
        /// </summary>
        /// <param name="username"></param>
        /// <param name="since"></param>
        /// <param name="pagenumber"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PageResponse<FmTrack>> GetRecentScrobbles(string username, DateTime since, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentScrobblesCommand(Auth, username, since)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<Station>> GetRecentStations(string username, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentStationsCommand(Auth, username)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<Shout>> GetShoutsAsync(string username, int pagenumber, int count = LastFm.DefaultPageLength)
        {
            var command = new GetUserShoutsCommand(Auth, username)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<User>> GetInfoAsync(string username)
        {
            var command = new GetUserInfoCommand(Auth, username);

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> AddShoutAsync(string recipient, string message)
        {
            var command = new AddShoutCommand(Auth, recipient, message);

            return await command.ExecuteAsync();
        }
    }
}