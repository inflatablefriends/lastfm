using System;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{

    public class Station
    {
        public string Name { get; set; }
        public Uri Url { get; set; }

        internal static Station ParseJToken(JToken token)
        {
            var s = new Station();

            s.Name = token.Value<string>("name");
            s.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            return s;
        }
    }
}