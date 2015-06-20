using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Album
{
    [ApiMethodName(LastMethodsNames.album_getTags)]
    internal class GetTagsByUserCommand : GetAsyncCommandBase<PageResponse<LastTag>>
    {
        public override string Method
        { get { return LastMethodsNames.album_getTags; } }

        public string ArtistName { get; set; }

        public string AlbumName { get; set; }

        public string Username { get; set; }

        public bool Autocorrect { get; set; }

        public GetTagsByUserCommand(ILastAuth auth, string artist, string album, string username)
            : base(auth)
        {
            ArtistName = artist;
            AlbumName = album;
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", ArtistName);
            Parameters.Add("album", AlbumName);
            Parameters.Add("user", Username);
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastTag>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var resultsToken = jtoken.SelectToken("tags");
                var itemsToken = resultsToken.SelectToken("tag");

                return PageResponse<LastTag>.CreateSuccessResponse(itemsToken, LastTag.ParseJToken);
            }
            else
            {
                return PageResponse<LastTag>.CreateErrorResponse(status);
            }
        }
    }
}
