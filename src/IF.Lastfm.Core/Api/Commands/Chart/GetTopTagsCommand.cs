using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Chart
{
    [ApiMethodName("chart.getTopTags")]
    internal class GetTopTagsCommand : GetAsyncCommandBase<PageResponse<LastTag>>
    {
        public GetTopTagsCommand(ILastAuth auth) : base(auth)
        {
        }

        public override void SetParameters()
        {
            // 28/05/16 Paging parameters don't actually seem to do anything
            AddPagingParameters();
            DisableCaching();
        }

        public override async Task<PageResponse<LastTag>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jo = JObject.Parse(json);
                var tagsToken = jo.SelectToken("tags.tag");
                var pageInfoToken = jo.SelectToken("@attr");

                return PageResponse<LastTag>.CreateSuccessResponse(tagsToken, pageInfoToken, LastTag.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return PageResponse<LastTag>.CreateErrorResponse(status);
            }
        }
    }
}