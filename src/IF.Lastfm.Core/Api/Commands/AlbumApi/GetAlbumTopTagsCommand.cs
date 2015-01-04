using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.AlbumApi
{
    internal class GetAlbumTopTagsCommand : GetAsyncCommandBase<PageResponse<LastTag>>
    {
        public string AlbumMbid { get; set; }

        public string ArtistName { get; set; }

        public string AlbumName { get; set; }

        public bool Autocorrect { get; set; }

        public GetAlbumTopTagsCommand(ILastAuth auth)
            : base(auth)
        {
            Method = "album.getTopTags";
        }

        public GetAlbumTopTagsCommand(ILastAuth auth, string album, string artist)
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

                return PageResponse<LastTag>.CreateSuccessResponse(itemsToken, resultsToken, LastTag.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTag>>(error);
            }
        }
    }
}
