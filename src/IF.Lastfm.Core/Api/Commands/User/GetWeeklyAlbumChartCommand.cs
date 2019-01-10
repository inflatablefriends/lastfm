using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getWeeklyAlbumChart")]
    internal class GetWeeklyAlbumChartCommand : GetAsyncCommandBase<PageResponse<LastAlbum>>
    {
        public string Username { get; set; }
        public double? From { get; set; }
        public double? To { get; set; }

        public GetWeeklyAlbumChartCommand(ILastAuth auth, string username)
            : base(auth)
        {
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            if(!From.Equals(null)) 
            {
                Parameters.Add("from", From.ToString());
            }
            if(!To.Equals(null)) 
            {
                Parameters.Add("to", To.ToString());
            }
            DisableCaching();
        }

        public override async Task<PageResponse<LastAlbum>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var albumListToken = jtoken.SelectToken("weeklyalbumchart");
                var itemsToken = albumListToken.SelectToken("album");
                var pageInfoToken = albumListToken.SelectToken("@attr");

                return PageResponse<LastAlbum>.CreateSuccessResponse(itemsToken, pageInfoToken, LastAlbum.ParseJToken, LastPageResultsType.Attr);
            }

            return LastResponse.CreateErrorResponse<PageResponse<LastAlbum>>(status);
        }
    }
}