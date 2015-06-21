using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Chart
{
    [ApiMethodName("chart.getTopArtists")]
    internal class GetTopArtistsCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public GetTopArtistsCommand(ILastAuth auth) : base(auth) { }

        public override void SetParameters()
        {
            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("artists");
                var itemsToken = jtoken.SelectToken("artist");
                var pageInfoToken = jtoken.SelectToken("@attr");

                return PageResponse<LastArtist>.CreateSuccessResponse(itemsToken, pageInfoToken, LastArtist.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(status);
            }


        }
    }
}
