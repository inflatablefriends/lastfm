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
    internal class GetTopArtistsCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public GetTopArtistsCommand(IAuth auth)
            : base(auth)
        {
            Method = "chart.getTopArtists";
        }

        public override void SetParameters()
        {
            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (!LastFm.IsResponseValid(json, out error) || !response.IsSuccessStatusCode)
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(error);

            var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("artists");
            return PageResponse<LastArtist>.CreatePageResponse(jtoken.SelectToken("artist"), jtoken.SelectToken("@attr"),
                LastArtist.ParseJToken);
        }
    }
}
