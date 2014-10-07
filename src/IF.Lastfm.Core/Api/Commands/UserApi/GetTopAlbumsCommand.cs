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
    internal class GetTopAlbumsCommand : GetAsyncCommandBase<PageResponse<LastAlbum>>
    {
        public string Username { get; set; }
        public LastStatsTimeSpan TimeSpan { get; set; }

        public GetTopAlbumsCommand(IAuth auth, string username, LastStatsTimeSpan span) : base(auth)
        {
            Method = "user.getTopAlbums";
            Username = username;
            TimeSpan = span;
        }

        public override void SetParameters()
        {
            Parameters.Add("username", Username);
            Parameters.Add("period", TimeSpan.GetApiName());

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastAlbum>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var albumsToken = jtoken.SelectToken("topalbums").SelectToken("album");

                var albums = albumsToken.Children().Select(LastAlbum.ParseJToken);

                return PageResponse<LastAlbum>.CreateSuccessResponse(albums);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(error);
            }
        }
    }
}