using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getRecommendedArtists")]
    internal class GetRecommendedArtistsCommand : PostAsyncCommandBase<PageResponse<LastArtist>>
    {
        public GetRecommendedArtistsCommand(ILastAuth auth) : base(auth) { }

        public override void SetParameters()
        {
            AddPagingParameters();
        }

        public override async Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var resultsToken = jtoken.SelectToken("recommendations");
                var itemsToken = resultsToken.SelectToken("artist");

                return PageResponse<LastArtist>.CreateSuccessResponse(itemsToken, resultsToken, LastArtist.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(status);
            }
        }
    }
}