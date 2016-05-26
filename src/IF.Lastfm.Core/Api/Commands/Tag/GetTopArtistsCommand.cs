using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Tag
{
    [ApiMethodName("tag.getTopArtists")]
    internal class GetTopArtistsCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public string TagName { get; set; }

        public GetTopArtistsCommand(ILastAuth auth, string tagName) : base(auth)
        {
            TagName = tagName;
        }

        public override void SetParameters()
        {
            Parameters.Add("tag", TagName);
            AddPagingParameters();
            DisableCaching();
        }

        public override async Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var jtoken = JsonConvert.DeserializeObject<JToken>(json);
            var resultsToken = jtoken.SelectToken("topartists");

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                if (string.IsNullOrEmpty(resultsToken.SelectToken("@attr.tag").Value<string>()))
                {
                    return PageResponse<LastArtist>.CreateErrorResponse(LastResponseStatus.MissingParameters);
                }

                var itemsToken = resultsToken.SelectToken("artist");

                return PageResponse<LastArtist>.CreateSuccessResponse(itemsToken, resultsToken, LastArtist.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                // The tag api always returns a "valid" response, so 
                return PageResponse<LastArtist>.CreateErrorResponse(status);
            }
        }
    }
}
