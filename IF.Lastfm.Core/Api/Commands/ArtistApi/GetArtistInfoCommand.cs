using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.ArtistApi
{
    internal class GetArtistInfoCommand : GetAsyncCommandBase<LastResponse<Artist>>
    {
        public string ArtistName { get; set; }
        public string BioLanguage { get; set; }
        public bool Autocorrect { get; set; }

        public GetArtistInfoCommand(IAuth auth, string artistname)
            : base(auth)
        {
            Method = "artist.getInfo";
            ArtistName = artistname;
        }

        public override Uri BuildRequestUrl()
        {
            var parameters = new Dictionary<string, string>
                {
                    {"artist", ArtistName},
                    {"autocorrect", Convert.ToInt32(Autocorrect).ToString()}
                };

            base.DisableCaching(parameters);

            var apiUrl = LastFm.FormatApiUrl(Method, Auth.ApiKey, parameters);
            return new Uri(apiUrl, UriKind.Absolute);
        }

        public async override Task<LastResponse<Artist>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var artist = Artist.ParseJToken(jtoken.SelectToken("artist"));

                return LastResponse<Artist>.CreateSuccessResponse(artist);
            }
            else
            {
                return LastResponse<Artist>.CreateErrorResponse(error);
            }
        }
    }
}
