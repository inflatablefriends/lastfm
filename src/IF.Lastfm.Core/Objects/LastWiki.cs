using Newtonsoft.Json.Linq;
using System;

namespace IF.Lastfm.Core.Objects
{
    /// <summary>
    /// TODO YearFormed -> FormationList
    /// "formationlist": {
    ///   "formation": {
    ///     "yearfrom": "2003",
    ///     "yearto": ""
    ///   }
    /// }
    /// 
    /// TODO links 
    /// "links": {
    ///   "link": {
    ///     "#text": "",
    ///     "rel": "original",
    ///     "href": "http://www.last.fm/music/Frightened+Rabbit/+wiki"
    ///   }
    /// }
    /// </summary>
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
