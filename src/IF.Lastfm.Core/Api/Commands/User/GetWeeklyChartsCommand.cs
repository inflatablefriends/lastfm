using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getWeeklyChartList")]
    internal class GetWeeklyChartsCommand : GetAsyncCommandBase<PageResponse<LastWeeklyChartList>>
    {
        public string Username { get; set; }

        public GetWeeklyChartsCommand(ILastAuth auth, string username)
            : base(auth)
        {
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            DisableCaching();
        }

        public override async Task<PageResponse<LastWeeklyChartList>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var chartListToken = jtoken.SelectToken("weeklychartlist");
                var itemsToken = chartListToken.SelectToken("chart");
                var pageInfoToken = chartListToken.SelectToken("@attr");

                return PageResponse<LastWeeklyChartList>.CreateSuccessResponse(itemsToken, pageInfoToken, LastWeeklyChartList.ParseJToken, LastPageResultsType.Attr);
            }

            return LastResponse.CreateErrorResponse<PageResponse<LastWeeklyChartList>>(status);
        }
    }
}
