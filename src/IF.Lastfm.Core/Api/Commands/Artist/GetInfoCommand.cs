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
    [ApiMethodName("artist.getInfo")]
    internal class GetInfoCommand : GetAsyncCommandBase<LastResponse<LastArtist>>
    {
        public string ArtistMbid { get; set; }

        public string ArtistName { get; set; }

        public string BioLanguage { get; set; }

        public bool Autocorrect { get; set; }

        public string UserName { get; set; }

        public GetInfoCommand(ILastAuth auth) : base(auth) { }

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
            
            if (BioLanguage != null)
            {
                Parameters.Add("lang", BioLanguage);
            }
            
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Parameters.Add("username", UserName);
            }

            DisableCaching();
        }

        public override async Task<LastResponse<LastArtist>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var artist = LastArtist.ParseJToken(jtoken.SelectToken("artist"));

                return LastResponse<LastArtist>.CreateSuccessResponse(artist);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastArtist>>(status);
            }
        }
    }
}
