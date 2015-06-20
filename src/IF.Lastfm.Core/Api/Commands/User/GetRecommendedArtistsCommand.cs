using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName(LastMethodsNames.user_getRecommendedArtists)]
    internal class GetRecommendedArtistsCommand : PostAsyncCommandBase<PageResponse<LastArtist>>
    {
        public override string Method
        { get { return LastMethodsNames.user_getRecommendedArtists; } }

        public GetRecommendedArtistsCommand(ILastAuth auth) : base(auth) { }

        public override void SetParameters()
        {
            AddPagingParameters();
        }

        public async override Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
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