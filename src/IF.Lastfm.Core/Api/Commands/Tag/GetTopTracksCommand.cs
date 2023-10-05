using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Tag
{
    [ApiMethodName("tag.getTopTracks")]
    internal class GetTopTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string TagName { get; set; }

        public GetTopTracksCommand(ILastAuth auth, string tagName) : base(auth)
        {
            TagName = tagName;
        }

        public override void SetParameters()
        {
            Parameters.Add("tag", TagName);
            AddPagingParameters();
            DisableCaching();
        }

        public override async Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var resultsToken = jtoken.SelectToken("tracks");
                var itemsToken = resultsToken.SelectToken("track");

                return PageResponse<LastTrack>.CreateSuccessResponse(itemsToken, resultsToken, LastTrack.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(status);
            }
        }
    }
}