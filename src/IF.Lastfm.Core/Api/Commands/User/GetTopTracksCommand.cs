using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getTopTracks")]
    internal class GetTopTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string Username { get; private set; }
        public LastStatsTimeSpan Period { get; set; }

        public GetTopTracksCommand(ILastAuth auth, string username) : base(auth)
        {
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            Parameters.Add("period", Period.ToString());

            AddPagingParameters();
            DisableCaching();
        }

        public override async Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("toptracks");
                var itemsToken = jtoken.SelectToken("track");
                var attrToken = jtoken.SelectToken("@attr");

                return PageResponse<LastTrack>.CreateSuccessResponse(itemsToken, attrToken, LastTrack.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(status);
            }
        }
    }
}
