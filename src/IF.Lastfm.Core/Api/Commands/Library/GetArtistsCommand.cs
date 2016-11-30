using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Library
{
    [ApiMethodName("library.getArtists")]
    internal class GetArtistsCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public string Username { get; }

        public GetArtistsCommand(ILastAuth auth, string username) : base(auth)
        {
            Username = username;
            Page = 1;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            
            AddPagingParameters();
            DisableCaching();
        }

        public override async Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("artists");
                var tracksToken = jtoken.SelectToken("artist");
                var pageInfoToken = jtoken.SelectToken("@attr");

                return PageResponse<LastArtist>.CreateSuccessResponse(tracksToken, pageInfoToken, LastArtist.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(status);
            }
        }
    }
}