using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Album
{
    [ApiMethodName("album.getInfo")]
    internal class GetInfoCommand : GetAsyncCommandBase<LastResponse<LastAlbum>>
    {
        public string AlbumMbid { get; set; }

        public string ArtistName { get; set; }

        public string AlbumName { get; set; }

        public string UserName { get; set; }

        public bool Autocorrect { get; set; }

        public GetInfoCommand(ILastAuth auth) : base(auth) { }

        public GetInfoCommand(ILastAuth auth, string album, string artist)
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

            if (UserName != null)
            {
                Parameters.Add("username", UserName);
            }

            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            DisableCaching();
        }

        public override async Task<LastResponse<LastAlbum>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var album = LastAlbum.ParseJToken(jtoken.SelectToken("album"));

                return LastResponse<LastAlbum>.CreateSuccessResponse(album);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastAlbum>>(status);
            }
        }
    }
}
