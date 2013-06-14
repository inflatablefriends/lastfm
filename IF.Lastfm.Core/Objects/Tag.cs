using System;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class Tag
    {
        #region Properties

        public string Name { get; set; }
        public Uri Url { get; set; }

        #endregion

        internal static Tag ParseJToken(JToken token)
        {
            var t = new Tag();

            t.Name = token.Value<string>("name");
            t.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            return t;
        }
    }
}