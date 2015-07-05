using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Tag
{
    [ApiMethodName("tag.getInfo")]
    public class GetInfoCommand:GetAsyncCommandBase<LastResponse<LastTag>>
    {
        public string TagName { get; set; }

        public GetInfoCommand(ILastAuth auth, string tagName) 
            : base(auth)
        {
            TagName = tagName;
        }

        public override void SetParameters()
        {
            Parameters.Add("tag", TagName);

            DisableCaching();
        }

        public async override Task<LastResponse<LastTag>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var tag = LastTag.ParseJToken(jtoken.SelectToken("tag"));

                return LastResponse<LastTag>.CreateSuccessResponse(tag);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastTag>>(status);
            }

        }
    }
}
