using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class GetUserInfoCommand : GetAsyncCommandBase<LastResponse<LastUser>>
    {
        public string Username { get; set; }

        public GetUserInfoCommand(ILastAuth auth, string username) : base(auth)
        {
            Method = "user.getInfo";
            Username = username;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);

            DisableCaching();
        }
        
        public async override Task<LastResponse<LastUser>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var userToken = jtoken.SelectToken("user");
                var user = LastUser.ParseJToken(userToken);

                return LastResponse<LastUser>.CreateSuccessResponse(user);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastUser>>(error);
            }
        }
    }
}