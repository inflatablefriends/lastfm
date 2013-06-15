using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class Artist
    {
        internal static string GetNameFromJToken(JToken artistToken)
        {
            var name = artistToken.Value<string>("name");

            if (string.IsNullOrEmpty(name))
            {
                name = artistToken.Value<string>("#text");
            }

            return name;
        }
    }
}