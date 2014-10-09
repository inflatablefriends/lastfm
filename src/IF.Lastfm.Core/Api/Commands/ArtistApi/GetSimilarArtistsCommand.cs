using System.Collections.Generic;
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
    internal class GetSimilarArtistsCommand : GetAsyncCommandBase<LastResponse<List<LastArtist>>>
    {
        public int Limit;
        public bool Autocorrect { get; set; }
        public string ArtistName { get; set; }

        public GetSimilarArtistsCommand(IAuth auth, string artistName, bool autocorrect, int limit)
            : base(auth)
        {
            Limit = limit;
            Autocorrect = autocorrect;
            Method = "artist.getSimilar";
            ArtistName = artistName;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);
            Parameters.Add("limit", Limit.ToString());
            Parameters.Add("autocorrect", Autocorrect.ToInt().ToString());
            DisableCaching();
        }

        public async override Task<LastResponse<List<LastArtist>>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var artists = jtoken.SelectToken("similarartists")
                    .SelectToken("artist")
                    .Children().Select(LastArtist.ParseJToken)
                    .ToList();

                var lastresponse = LastResponse<List<LastArtist>>.CreateSuccessResponse(artists);
                return lastresponse;
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<List<LastArtist>>>(error);
            }
        }
    }
}
