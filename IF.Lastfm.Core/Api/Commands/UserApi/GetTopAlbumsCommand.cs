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

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetTopAlbumsCommand : GetAsyncCommandBase<PageResponse<Album>>
    {
        public string Username { get; set; }
        public LastStatsTimeSpan TimeSpan { get; set; }

        public GetTopAlbumsCommand(IAuth auth, string username, LastStatsTimeSpan span) : base(auth)
        {
            Method = "user.getTopAlbums";
            Username = username;
            TimeSpan = span;
        }

        public override Uri BuildRequestUrl()
        {
            var parameters = new Dictionary<string, string>
                             {
                                 {"username", Username},
                                 {"period", TimeSpan.GetApiName()}
                             };

            base.AddPagingParameters(parameters);

            var uristring = LastFm.FormatApiUrl(Method, Auth.ApiKey, parameters);
            return new Uri(uristring, UriKind.Absolute);
        }

        public async override Task<PageResponse<Album>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var albumsToken = jtoken.SelectToken("topalbums").SelectToken("album");

                var albums = albumsToken.Children().Select(Album.ParseJToken);

                return PageResponse<Album>.CreateSuccessResponse(albums);
            }
            else
            {
                return PageResponse<Album>.CreateErrorResponse(error);
            }
        }
    }
}