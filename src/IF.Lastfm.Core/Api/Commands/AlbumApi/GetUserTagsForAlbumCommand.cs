using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.AlbumApi
{
    internal class GetUserTagsForAlbumCommand: GetAsyncCommandBase<PageResponse<LastTag>>
    {
        public string AlbumMbid { get; set; }

        public string ArtistName { get; set; }

        public string AlbumName { get; set; }

        public string UserName { get; set; }
        public bool Autocorrect { get; set; }

        public GetUserTagsForAlbumCommand(ILastAuth auth, string album, string artist, string username)
            : base(auth)
        {
            Method = "album.getTags";

            AlbumName = album;
            ArtistName = artist;
            UserName = username;
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
                Parameters.Add("user", UserName);
            }
            Parameters.Add("autocorrect", Convert.ToInt32(Autocorrect).ToString());

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastTag>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var resultsToken = jtoken.SelectToken("toptags");
                var itemsToken = resultsToken.SelectToken("tag");

                return PageResponse<LastTag>.CreateSuccessResponse(itemsToken, resultsToken, LastTag.ParseJToken, false);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTag>>(error);
            }
        }
    }
}
