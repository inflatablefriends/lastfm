using System;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class LastTag : ILastfmObject
    {
        #region Properties

        public string Name { get; set; }
        
        public Uri Url { get; set; }

        public int? Count { get; set; }

        #endregion

        public LastTag()
        {
        }

        public LastTag(string name, string uri, int? count = null)
        {
            Name = name;
            Url = new Uri(uri, UriKind.RelativeOrAbsolute);
            Count = count;
        }

        internal static LastTag ParseJToken(JToken token)
        {
            var name = token.Value<string>("name");
            var url = token.Value<string>("url");

            int? count = null;
            var countToken = token.SelectToken("count");
            if (countToken != null)
            {
                count = countToken.ToObject<int?>();
            }

            return new LastTag(name, url, count);
        }
    }
}