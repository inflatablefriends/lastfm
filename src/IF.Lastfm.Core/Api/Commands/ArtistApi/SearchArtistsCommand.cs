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
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (!LastFm.IsResponseValid(json, out error) || !response.IsSuccessStatusCode)
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(error);

            var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("results");
            return PageResponse<LastArtist>.CreatePageResponse(jtoken.SelectToken("artistsmatches").SelectToken("artist"),
                jtoken, LastArtist.ParseJToken, true);
        }
    }
}
