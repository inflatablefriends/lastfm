using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetRecommendedArtistsCommand : PostAsyncCommandBase<PageResponse<LastArtist>>
    {
        public GetRecommendedArtistsCommand(ILastAuth auth) : base(auth)
        {
            Method = "user.getRecommendedArtists";
        }

        public override void SetParameters()
        {
            AddPagingParameters();
        }

        public async override Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var resultsToken = jtoken.SelectToken("recommendations");

                return PageResponse<LastArtist>.CreateSuccessResponse(resultsToken.SelectToken("artist"), resultsToken.SelectToken("@attr"), LastArtist.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(error);
            }
        }
    }
}