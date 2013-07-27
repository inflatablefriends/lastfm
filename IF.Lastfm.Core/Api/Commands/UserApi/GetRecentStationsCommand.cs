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
    internal class GetRecentStationsCommand : PostAsyncCommandBase<PageResponse<Station>>
    {
        public string Username { get; private set; }

        public GetRecentStationsCommand(IAuth auth, string username) : base(auth)
        {
            Method = "user.getRecentStations";
            Username = username;
        }

        public async override Task<PageResponse<Station>> ExecuteAsync()
        {
            var parameters = new Dictionary<string, string>
                             {
                                 {"user", Username}
                             };

            AddPagingParameters(parameters);

            return await ExecuteInternal(parameters);
        }

        public async override Task<PageResponse<Station>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("recentstations");

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