using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class LastWiki : ILastfmObject
    {
        #region Properties

        public DateTime Published { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public int YearFormed { get; set; }

        #endregion

        internal static LastWiki ParseJToken(JToken token)
        {
            return new LastWiki
            {
                Published = token.Value<DateTime>("published"),
                Summary = token.Value<string>("summary").Trim(),
                Content = token.Value<string>("content").Trim(),
                YearFormed = token.Value<int>("yearformed")
            };
        }
    }
}
