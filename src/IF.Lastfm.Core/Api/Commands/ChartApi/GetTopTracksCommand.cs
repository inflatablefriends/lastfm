using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.ChartApi
{
    internal class GetTopTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public GetTopTracksCommand(IAuth auth)
            : base(auth)
        {
            Method = "chart.getTopTracks";
        }

        public override void SetParameters()
        {
            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (!LastFm.IsResponseValid(json, out error) || !response.IsSuccessStatusCode)
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(error);

            var jtoken = JsonConvert.DeserializeObject<JToken>(json);
            return PageResponse<LastTrack>.CreatePageResponse(jtoken.SelectToken("tracks").SelectToken("track"), jtoken.SelectToken("@attr"), LastTrack.ParseJToken);
        }
    }
}
