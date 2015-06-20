using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName("user.getInfo")]
    internal class GetInfoCommand : GetAsyncCommandBase<LastResponse<LastUser>>
    {
        public string Username { get; set; }

        public GetInfoCommand(ILastAuth auth, string username) : base(auth)
        {
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

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json);
                var userToken = jtoken.SelectToken("user");
                var user = LastUser.ParseJToken(userToken);

                return LastResponse<LastUser>.CreateSuccessResponse(user);
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse<LastUser>>(status);
            }
        }
    }
}