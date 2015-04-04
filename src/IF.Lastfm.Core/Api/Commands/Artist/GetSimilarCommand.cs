using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Artist
{
    internal class GetSimilarCommand : GetAsyncCommandBase<PageResponse<LastArtist>>
    {
        public bool Autocorrect { get; set; }

        public string ArtistName { get; set; }

        public int? Limit { get; set; }

        public GetSimilarCommand(ILastAuth auth, string artistName)
            : base(auth)
        {
            Method = "artist.getSimilar";

            ArtistName = artistName;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            if (Limit != null)
            {
                Parameters.Add("limit", Limit.ToString());
            }

            DisableCaching();
        }

        public async override Task<PageResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var itemsToken = jtoken.SelectToken("similarartists").SelectToken("artist");

                return PageResponse<LastArtist>.CreateSuccessResponse(itemsToken, LastArtist.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastArtist>>(error);
            }
        }
    }
}
