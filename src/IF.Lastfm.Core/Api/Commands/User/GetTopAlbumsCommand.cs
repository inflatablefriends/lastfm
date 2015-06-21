using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getTopAlbums")]
    internal class GetTopAlbumsCommand : GetAsyncCommandBase<PageResponse<LastAlbum>>
    {
        public string Username { get; set; }
        public LastStatsTimeSpan TimeSpan { get; set; }

        public GetTopAlbumsCommand(ILastAuth auth, string username, LastStatsTimeSpan span) : base(auth)
        {
            Username = username;
            TimeSpan = span;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            Parameters.Add("period", TimeSpan.GetApiName());

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastAlbum>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JToken.Parse(json);
                var itemsToken = jtoken.SelectToken("topalbums").SelectToken("album");
                var pageInfoToken = jtoken.SelectToken("@attr");

                return PageResponse<LastAlbum>.CreateSuccessResponse(itemsToken, pageInfoToken, LastAlbum.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(status);
            }

        }
    }
}