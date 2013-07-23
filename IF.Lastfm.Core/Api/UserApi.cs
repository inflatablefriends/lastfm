using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.UserApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<PageResponse<Album>> GetTopAlbums(string username, LastStatsTimeSpan span, int pagenumber = 0, int count = LastFm.DefaultPageLength)
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
        public async Task<PageResponse<Track>> GetRecentScrobbles(string username, DateTime since, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetRecentScrobblesCommand(Auth, username, since)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<Station>> GetRecentStations(int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            const string apiMethod = "user.getRecentStations";

            var methodParameters = new Dictionary<string, string>
                                       {
                                           {"user", Auth.User.Username},
                                           {"page", pagenumber.ToString()},
                                           {"limit", count.ToString()},
                                           {"sk", Auth.User.Token}
                                       };

            var apisig = Auth.GenerateMethodSignature(apiMethod, methodParameters);

            var postContent = LastFm.CreatePostBody(apiMethod,
                Auth.ApiKey,
                apisig,
                methodParameters);

            var httpClient = new HttpClient();
            var lastResponse = await httpClient.PostAsync(LastFm.ApiRoot, postContent);
            var json = await lastResponse.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && lastResponse.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("recentstations");

                var stationsToken = jtoken.SelectToken("station");

                var stations = stationsToken.Children().Select(Station.ParseJToken).ToList();

                var pageresponse = PageResponse<Station>.CreateSuccessResponse(stations);

                var attrToken = jtoken.SelectToken("@attr");
                pageresponse.AddPageInfoFromJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return PageResponse<Station>.CreateErrorResponse(error);
            }
        }
    }
}