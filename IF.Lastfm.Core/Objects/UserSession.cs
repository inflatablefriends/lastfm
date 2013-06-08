using IF.Lastfm.Core.Json;
using Newtonsoft.Json;

namespace IF.Lastfm.Core
{
    public class UserSession
    {
        [JsonProperty("name")]
        public string Username { get; set; }
        
        [JsonProperty("key")]
        public string Token { get; set; }

        [JsonProperty("subscriber"), JsonConverter(typeof(LastFmBooleanConverter))]
        public bool IsSubscriber { get; set; }
    }
}