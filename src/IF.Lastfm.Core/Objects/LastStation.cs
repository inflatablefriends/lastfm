using System;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class LastStation : ILastfmObject
    {
        public string Name { get; set; }
        public Uri Url { get; set; }

        internal static LastStation ParseJToken(JToken token)
        {
            var s = new LastStation();

            s.Name = token.Value<string>("name");
            s.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            return s;
        }
    }
}