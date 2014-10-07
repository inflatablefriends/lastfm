using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.ArtistApi
{
    internal class SearchArtistsCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public string ArtistName { get; set; }

        public SearchArtistsCommand(IAuth auth, string artistName)
            : base(auth)
        {
            Method = "artist.search";
            ArtistName = artistName;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var artists = jtoken.SelectToken("results")
                    .SelectToken("artistmatches")
                    .SelectToken("artist")
                    .Children().Select(LastArtist.ParseJToken)
                    .ToList();

                var pageresponse = PageResponse<LastArtist>.CreateSuccessResponse(artists);

                var attrToken = jtoken.SelectToken("results");
                pageresponse.AddPageInfoFromOpenQueryJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(error);
            }
        }
    }
}
