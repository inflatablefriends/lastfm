using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.ArtistApi
{
    public class GetArtistShoutsCommand : GetAsyncCommandBase<PageResponse<Shout>>
    {
        public string ArtistName { get; set; }
        public bool Autocorrect { get; set; }

        public GetArtistShoutsCommand(IAuth auth, string artistname)
            : base(auth)
        {
            Method = "artist.getShouts";
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            base.AddPagingParameters();
            base.DisableCaching();
        }

        public override async Task<PageResponse<Shout>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("shouts");
                return PageResponse<Shout>.CreatePageResponse(jtoken.SelectToken("shout"), jtoken.SelectToken("@attr"), Shout.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<Shout>>(error);
            }
        }
    }
}