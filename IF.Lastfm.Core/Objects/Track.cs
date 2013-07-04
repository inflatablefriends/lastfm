using System;
using System.Collections.Generic;
using System.Linq;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    /// <summary>
    /// TODO Wiki, Stream availability
    /// </summary>
    public class Track : ILastFmObject
    {
        #region Properties

        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string Mbid { get; set; }
        public string ArtistName { get; set; }
        public string ArtistMbid { get; set; }
        public Uri Url { get; set; }
        public LastImageCollection Images { get; set; }
        
        public string AlbumName { get; set; }

        public int? ListenerCount { get; set; }
        public int? TotalPlayCount { get; set; }
        public IEnumerable<Tag> TopTags { get; set; }

        public DateTime? TimePlayed { get; set; }

        #endregion

        /// <summary>
        /// Parses the given JToken into a track
        /// </summary>
        /// <param name="token">A valid JToken</param>
        /// <returns>track equivalent to the JToken</returns>
        /// <remarks>If this method is used directly then the duration attribute will be parsed as MILLIseconds</remarks>
        internal static Track ParseJToken(JToken token)
        {
            var t = new Track();

            t.Name = token.Value<string>("name");
            t.Mbid = token.Value<string>("mbid");
            t.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);
            
            var artistToken = token.SelectToken("artist");
            if (artistToken != null)
            {
                t.ArtistName = Artist.GetNameFromJToken(artistToken);
                t.ArtistMbid = artistToken.Value<string>("mbid");
            }

            var albumToken = token.SelectToken("album");
            if (albumToken != null)
            {
                t.AlbumName = Album.GetNameFromJToken(albumToken);
            }

            var tagsToken = token.SelectToken("toptags");
            if (tagsToken != null)
            {
                t.TopTags = tagsToken.SelectToken("tag").Children().Select(Tag.ParseJToken);
            }

            var date = token.SelectToken("date");
            if (date != null)
            {
                var stamp = date.Value<double>("uts");
                t.TimePlayed = stamp.ToDateTimeUtc();
            }

            var images = token.SelectToken("image");
            if (images != null)
            {
                var imageCollection = LastImageCollection.ParseJToken(images);
                t.Images = imageCollection;
            }

            // api returns milliseconds when track.getInfo is called directly
            var secs = token.Value<double>("duration");
            if (Math.Abs(secs - default(double)) > double.Epsilon)
            {
                t.Duration = TimeSpan.FromMilliseconds(secs);
            }

            return t;
        }

        /// <summary>
        /// Parses the given JToken into a track
        /// </summary>
        /// <param name="token">A valid JToken</param>
        /// <param name="albumName">Name of the album this track belongs to</param>
        /// <returns>track equivalent to the JToken</returns>
        /// <remarks>If this method is used then the duration attribute will be parsed as seconds</remarks>
        internal static Track ParseJToken(JToken token, string albumName)
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