using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Tag
{
    [ApiMethodName("tag.getSimilar")]
    internal class GetSimilarCommand : GetAsyncCommandBase<PageResponse<LastTag>>
    {
        public string TagName { get; set; }

        public GetSimilarCommand(ILastAuth auth, string tagname)
            : base(auth)
        {
            TagName = tagname;
        }

        public override void SetParameters()
        {
            Parameters.Add("tag", TagName);
            
            DisableCaching();
        }

        public override async Task<PageResponse<LastTag>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("similartags");
                var itemsToken = jtoken.SelectToken("tag");
                var attrToken = jtoken.SelectToken("@attr");
                var relatedTag = attrToken.SelectToken("tag").Value<string>();

                return PageResponse<LastTag>.CreateSuccessResponse(itemsToken, jt => LastTag.ParseJToken(jt, relatedTag));
            }

            return LastResponse.CreateErrorResponse<PageResponse<LastTag>>(status);
        }
    }
}
