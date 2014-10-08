using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetUserShoutsCommand : GetAsyncCommandBase<PageResponse<Shout>>
    {
        public string Username { get; set; }

        public GetUserShoutsCommand(IAuth auth, string username) : base(auth)
        {
            Method = "user.getShouts";
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<Shout>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                JToken jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("shouts");
                return PageResponse<Shout>.CreatePageResponse(jtoken.SelectToken("shout"), jtoken.SelectToken("@attr"), Shout.ParseJToken);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<Shout>>(error);
            }
        }
    }
}