using System;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class LastTag : ILastfmObject
    {
        #region Properties

        public string Name { get; set; }
        public Uri Url { get; set; }

        #endregion

        internal static LastTag ParseJToken(JToken token)
        {
            var t = new LastTag();

            t.Name = token.Value<string>("name");
            t.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            return t;
        }
    }
}