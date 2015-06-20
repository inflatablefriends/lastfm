using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getRecentStations")]
    internal class GetRecentStationsCommand : PostAsyncCommandBase<PageResponse<LastStation>>
    {
        public string Username { get; private set; }

        public GetRecentStationsCommand(ILastAuth auth, string username) : base(auth)
        {
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);

            AddPagingParameters();
        }

        public async override Task<PageResponse<LastStation>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("recentstations");

                var stationsToken = jtoken.SelectToken("station");

                var stations = stationsToken.Children().Select(LastStation.ParseJToken).ToList();

                var pageresponse = PageResponse<LastStation>.CreateSuccessResponse(stations);

                var attrToken = jtoken.SelectToken("@attr");
                pageresponse.AddPageInfoFromJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastStation>>(status);
            }
        }
    }
}