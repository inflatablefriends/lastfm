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

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class GetShoutsCommand : GetAsyncCommandBase<PageResponse<Shout>>
    {
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
        public bool Autocorrect { get; set; }

        public GetShoutsCommand(IAuth auth, string trackname, string artistname) : base(auth)
        {
            Method = "track.getShouts";
            TrackName = trackname;
            ArtistName = artistname;
        }

        public async override Task<PageResponse<Shout>> ExecuteAsync()
        {
            var parameters = new Dictionary<string, string>
                {
                    {"track", TrackName},
                    {"artist", ArtistName},
                    {"autocorrect", Convert.ToInt32(Autocorrect).ToString()}
                };

            base.AddPagingParameters(parameters);

            var apiUrl = LastFm.FormatApiUrl(Method, Auth.ApiKey, parameters);
            Url = new Uri(apiUrl, UriKind.Absolute);

            return await ExecuteInternal();
        }

        public async override Task<PageResponse<Shout>> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("shouts");

                var shoutsToken = jtoken.SelectToken("shout");

                var shouts = new List<Shout>();
                foreach (var shout in shoutsToken.Children())
                {
                    var s = Shout.ParseJToken(shout);
                    shouts.Add(s);
                }

                var pageresponse = PageResponse<Shout>.CreateSuccessResponse(shouts);

                var attrToken = jtoken.SelectToken("@attr");
                pageresponse.AddPageInfoFromJToken(attrToken);

                return pageresponse;
            }
            else
            {
                return PageResponse<Shout>.CreateErrorResponse(error);
            }
        }
    }
}
