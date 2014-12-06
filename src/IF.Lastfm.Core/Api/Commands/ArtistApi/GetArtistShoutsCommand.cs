using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.ArtistApi
{
    internal class GetArtistShoutsCommand : GetAsyncCommandBase<PageResponse<LastShout>>
    {
        public string ArtistName { get; set; }
        public bool Autocorrect { get; set; }

        public GetArtistShoutsCommand(ILastAuth auth, string artistname)
            : base(auth)
        {
            Method = "artist.getShouts";
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            AddPagingParameters();
            DisableCaching();
        }

        public override async Task<PageResponse<LastShout>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var shoutsToken = jtoken.SelectToken("shouts");
                var itemsToken = shoutsToken.SelectToken("shout");
                var pageInfoToken = shoutsToken.SelectToken("@attr");

                return PageResponse<LastShout>.CreateSuccessResponse(itemsToken, pageInfoToken, LastShout.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastShout>>(error);
            }
        }
    }
}