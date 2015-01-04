using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.AlbumApi
{
    internal class GetAlbumShoutsCommand : GetAsyncCommandBase<PageResponse<LastShout>>
    {
        public string AlbumName { get; set; }

        public string ArtistName { get; set; }

        public bool Autocorrect { get; set; }

        public GetAlbumShoutsCommand(ILastAuth auth, string albumname, string artistname)
            : base(auth)
        {
            Method = "album.getShouts";

            AlbumName = albumname;
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("album", AlbumName);
            Parameters.Add("artist", ArtistName);
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            AddPagingParameters();
            DisableCaching();
        }
        
        public async override Task<PageResponse<LastShout>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("shouts");
                var itemsToken = jtoken.SelectToken("shout");
                var pageInfoToken = jtoken.SelectToken("@attr");

                return PageResponse<LastShout>.CreateSuccessResponse(itemsToken, pageInfoToken, LastShout.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastShout>>(error);
            }
        }
    }
}