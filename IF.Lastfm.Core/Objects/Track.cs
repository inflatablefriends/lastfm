using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    /// <summary>
    /// TODO Wiki, Images, Stream availability
    /// </summary>
    public class Track
    {
        #region Properties

        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string Mbid { get; set; }
        public string ArtistName { get; set; }
        public Uri Url { get; set; }
        
        public string AlbumName { get; set; }

        public int? ListenerCount { get; set; }
        public int? TotalPlayCount { get; set; }
        public IEnumerable<Tag> TopTags { get; set; }

        #endregion

        /// <summary>
        /// Parses the given JToken into a track
        /// </summary>
        /// <param name="token">A valid JToken</param>
        /// <returns>track equivalent to the JToken</returns>
        /// <remarks>If this method is used directly then the duration attribute will be parsed as MILLIseconds</remarks>
        public static Track ParseJToken(JToken token)
        {
            var t = new Track();

            t.Name = token.Value<string>("name");
            t.Mbid = token.Value<string>("mbid");
            t.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);
            t.ArtistName = token.SelectToken("artist").Value<string>("name");
            
            var tagsToken = token.SelectToken("toptags").SelectToken("tag");
            t.TopTags = tagsToken.Children().Select(Tag.ParseJToken);

            // api returns milliseconds when track.getInfo is called directly
            var secs = token.Value<double>("duration");
            t.Duration = TimeSpan.FromMilliseconds(secs);

            return t;
        }

        /// <summary>
        /// Parses the given JToken into a track
        /// </summary>
        /// <param name="token">A valid JToken</param>
        /// <param name="albumName">Name of the album this track belongs to</param>
        /// <returns>track equivalent to the JToken</returns>
        /// <remarks>If this method is used then the duration attribute will be parsed as seconds</remarks>
        public static Track ParseJToken(JToken token, string albumName)
        {
            var t = ParseJToken(token);
            t.AlbumName = albumName;
            
            // the api returns seconds for this value when not track.getInfo
            var secs = token.Value<double>("duration");
            t.Duration = TimeSpan.FromSeconds(secs);

            return t;
        }
    }
}