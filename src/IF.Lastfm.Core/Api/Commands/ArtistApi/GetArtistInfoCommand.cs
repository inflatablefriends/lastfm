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
    internal class GetArtistInfoCommand : GetAsyncCommandBase<LastResponse<LastArtist>>
    {
        public string ArtistMbid { get; set; }

        public string ArtistName { get; set; }

        public string BioLanguage { get; set; }

        public bool Autocorrect { get; set; }

        public GetArtistInfoCommand(IAuth auth)
            : base(auth)
        {
            Method = "artist.getInfo";
        }

        /// <summary>
        /// TODO Bio language
        /// </summary>
        public override void SetParameters()
        {
            if (ArtistMbid != null)
            {
                Parameters.Add("mbid", ArtistMbid);
            }
            else
            {
                Parameters.Add("artist", ArtistName);
            }

            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            DisableCaching();
        }

        public async override Task<LastResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var artist = LastArtist.ParseJToken(jtoken.SelectToken("artist"));

                return LastResponse<LastArtist>.CreateSuccessResponse(artist);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastArtist>>(error);
            }
        }
    }
}
