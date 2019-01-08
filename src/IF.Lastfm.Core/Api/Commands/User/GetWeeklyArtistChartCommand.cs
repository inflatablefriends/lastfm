using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getWeeklyArtistChart")]
    internal class GetWeeklyArtistChartCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public string Username { get; set; }
        public string To { get; set; }
        public string From { get; set; }

        public GetWeeklyArtistChartCommand(ILastAuth auth, string username, string from, string to)
            : base(auth)
        {
            Username = username;
            From = from;
            To = to; 
        }

        public override void SetParameters()
        {
            if(!From.Equals(null)) 
            {
                Parameters.Add("from", From);
            }
            if(!To.Equals(null)) 
            {
                Parameters.Add("to", To);
            }
            
            Parameters.Add("user", Username);
            DisableCaching();
        }

        public override async Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var chartListToken = jtoken.SelectToken("weeklyartistchart");
                var itemsToken = chartListToken.SelectToken("artist");
                var pageInfoToken = chartListToken.SelectToken("@attr");

                return PageResponse<LastArtist>.CreateSuccessResponse(itemsToken, pageInfoToken, LastArtist.ParseJToken, LastPageResultsType.Attr);
            }

            return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(status);
        }
    }
}