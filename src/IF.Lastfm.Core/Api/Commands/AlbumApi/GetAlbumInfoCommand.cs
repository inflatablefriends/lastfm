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
    internal class GetAlbumInfoCommand : GetAsyncCommandBase<LastResponse<LastAlbum>>
    {
        public string AlbumMbid { get; set; }

        public string ArtistName { get; set; }

        public string AlbumName { get; set; }

        public bool Autocorrect { get; set; }

        public GetAlbumInfoCommand(ILastAuth auth)
            : base(auth)
        {
            Method = "album.getInfo";
        }

        public GetAlbumInfoCommand(ILastAuth auth, string album, string artist)
            : this(auth)
        {
            AlbumName = album;
            ArtistName = artist;
        }

        public override void SetParameters()
        {
            if (AlbumMbid != null)
            {
                Parameters.Add("mbid", AlbumMbid);
            }
            else
            {
                Parameters.Add("artist", ArtistName);
                Parameters.Add("album", AlbumName);
            }

            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            DisableCaching();
        }

        public async override Task<LastResponse<LastAlbum>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var album = LastAlbum.ParseJToken(jtoken.SelectToken("album"));

                return LastResponse<LastAlbum>.CreateSuccessResponse(album);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastAlbum>>(error);
            }
        }
    }

}
