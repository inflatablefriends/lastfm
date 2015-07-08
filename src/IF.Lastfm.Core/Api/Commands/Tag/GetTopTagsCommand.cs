using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Tag
{
    [ApiMethodName("tag.getTopTags")]
    public class GetTopTagsCommand : GetAsyncCommandBase<PageResponse<LastTag>>
    {
        public GetTopTagsCommand(ILastAuth auth)
            : base(auth)
        {
        }

        public override void SetParameters()
        {
            DisableCaching();
        }

        public async override Task<PageResponse<LastTag>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var itemsToken = jtoken.SelectToken("toptags.tag");

                return PageResponse<LastTag>.CreateSuccessResponse(itemsToken, LastTag.ParseJToken);
            }

            return PageResponse<LastTag>.CreateErrorResponse(status);
        }
    }
}