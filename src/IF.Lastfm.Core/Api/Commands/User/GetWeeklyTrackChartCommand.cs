using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getWeeklyTrackChart")]
    internal class GetWeeklyTrackChartCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string Username { get; set; }
        public double? From { get; set; }
        public double? To { get; set; }

        public GetWeeklyTrackChartCommand(ILastAuth auth, string username)
            : base(auth)
        {
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            if(From != null) 
            {
                Parameters.Add("from", From.ToString());
            }
            if(To != null) 
            {
                Parameters.Add("to", To.ToString());
            }
            DisableCaching();
        }

        public override async Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var chartListToken = jtoken.SelectToken("weeklytrackchart");
                var itemsToken = chartListToken.SelectToken("track");
                var pageInfoToken = chartListToken.SelectToken("@attr");

                return PageResponse<LastTrack>.CreateSuccessResponse(itemsToken, pageInfoToken, LastTrack.ParseJToken, LastPageResultsType.Attr);
            }

            return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(status);
        }
    }
}