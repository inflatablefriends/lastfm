using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api
{
    public class AlbumApi : IAlbumApi
    {
        public IAuth Auth { get; private set; }

        public AlbumApi(IAuth auth)
        {
            Auth = auth;
        }

        #region album.getInfo

        public async Task<LastResponse<Album>> GetAlbumInfoAsync(string artistname, string albumname, bool autocorrect = false)
        {
            const string apiMethod = "album.getInfo";

            var parameters = new Dictionary<string, string>
                {
                    {"artist", artistname},
                    {"album", albumname},
                    {"autocorrect", Convert.ToInt32(autocorrect).ToString()}
                };

            var httpClient = new HttpClient();
            var apiUrl = LastFm.FormatApiUrl(apiMethod, Auth.ApiKey, parameters);
            var lastResponse = await httpClient.GetAsync(apiUrl);
            var json = await lastResponse.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && lastResponse.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);

                var album = Album.ParseJToken(jtoken.SelectToken("album"));

                return LastResponse<Album>.CreateSuccessResponse(album);
            }
            else
            {
                return LastResponse<Album>.CreateErrorResponse(error);
            }
        }

        public Task<LastResponse<Album>> GetAlbumInfoWithMbidAsync(string mbid, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region album.getBuylinks

        public Task<ListResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist, string album, CountryCode country, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<ListResponse<BuyLink>> GetBuyLinksForAlbumWithMbidAsync(string mbid, CountryCode country, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region album.getShouts

        public Task<ListResponse<Shout>> GetShoutsForAlbumAsync(string artist, string album, bool autocorrect = false, int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
        }

        public Task<ListResponse<Shout>> GetShoutsForAlbumWithMbidAsync(string mbid, bool autocorrect = false, int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region album.getTags

        public Task<ListResponse<Tag>> GetUserTagsForAlbumAsync(string artist, string album, string username, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<ListResponse<Tag>> GetUserTagsForAlbumWithMbidAsync(string mbid, string username, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region album.getTopTags

        public Task<ListResponse<Tag>> GetTopTagsForAlbumAsync(string artist, string album, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        public Task<ListResponse<Tag>> GetTopTagsForAlbumWithMbidAsync(string mbid, bool autocorrect = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region album.search

        public Task<ListResponse<Album>> SearchForAlbumAsync(string album, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}