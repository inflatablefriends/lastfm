using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api
{
    public interface IUserApi
    {
        IAuth Auth { get; }

        Task<IEnumerable<Album>> GetTopAlbums(LastStatsTimeSpan span,
                                              int startIndex = 0,
                                              int endIndex = LastFm.DefaultPageLength);
    }

    public class UserApi : IUserApi
    {
        public IAuth Auth { get; private set; }

        public UserApi(IAuth auth)
        {
            Auth = auth;
        }

        public async Task<IEnumerable<Album>> GetTopAlbums(LastStatsTimeSpan span, int startIndex = 0, int amount = LastFm.DefaultPageLength)
        {
            const string apiMethod = "user.getTopAlbums";

            var parameters = new Dictionary<string, string>
                {
                    {"user", Auth.User.Username},
                    {"period", span.GetApiName()}
                };

            var apiUrl = LastFm.FormatApiUrl(apiMethod, Auth.ApiKey, parameters);

            var httpClient = new HttpClient();

            var lastResponse = await httpClient.GetAsync(apiUrl);

            if (lastResponse.IsSuccessStatusCode)
            {
                var json = await lastResponse.Content.ReadAsStringAsync();

                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var albumsToken = jtoken.SelectToken("topalbums").SelectToken("album");

                return albumsToken.Children().Select(Album.ParseJToken);
            }
            else
            {
                // ???

                throw new NotImplementedException();
            }
        }
    }
}