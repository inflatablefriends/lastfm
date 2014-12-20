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
    public class LastTrack : ILastfmObject
    {
        #region Properties

        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string Mbid { get; set; }
        public string ArtistName { get; set; }
        public string ArtistMbid { get; set; }
        public Uri Url { get; set; }
        public LastImageSet Images { get; set; }
        
        public string AlbumName { get; set; }

        public int? ListenerCount { get; set; }
        public int? TotalPlayCount { get; set; }
        public IEnumerable<LastTag> TopTags { get; set; }

        public DateTime? TimePlayed { get; set; }
        public bool? IsLoved { get; set; }
        public bool? IsNowPlaying { get; set; }
        public int? Rank { get; set; }

        #endregion

        public LastTrack()
        {
        }

        /// <summary>
        /// Parses the given JToken into a track
        /// </summary>
        /// <param name="token">A valid JToken</param>
        /// <returns>track equivalent to the JToken</returns>
        /// <remarks>If this method is used directly then the duration attribute will be parsed as MILLIseconds</remarks>
        internal static LastTrack ParseJToken(JToken token)
        {
            var t = new LastTrack();

            t.Id = token.Value<string>("id");
            t.Name = token.Value<string>("name");
            t.Mbid = token.Value<string>("mbid");

            //0 to null
            t.TotalPlayCount = token.Value<int?>("playcount");

            t.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            var artistToken = token.SelectToken("artist");
            if (artistToken.Type != JTokenType.String)
            {
                t.ArtistName = LastArtist.GetNameFromJToken(artistToken);
                t.ArtistMbid = artistToken.Value<string>("mbid");
            }
            else
                t.ArtistName = artistToken.ToObject<string>();

            var albumToken = token.SelectToken("album");
            if (albumToken != null)
            {
                t.AlbumName = LastAlbum.GetNameFromJToken(albumToken);
            }

            var tagsToken = token.SelectToken("toptags");
            if (tagsToken != null)
            {
                var tagToken = tagsToken.SelectToken("tag");
                if (tagToken != null)
                {
                    t.TopTags =
                        tagToken.Type == JTokenType.Array
                        ? tagToken.Children().Select(LastTag.ParseJToken)
                        : new List<LastTag> { LastTag.ParseJToken(tagToken) };
                }
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
                var imageCollection = LastImageSet.ParseJToken(images);
                t.Images = imageCollection;
            }

            var lovedToken = token.SelectToken("userloved");
            if (lovedToken != null)
            {
                t.IsLoved = Convert.ToBoolean(lovedToken.Value<int>());
            }
            var attrToken = token.SelectToken("@attr");
            if (attrToken != null && attrToken.HasValues)
            {
                t.IsNowPlaying = attrToken.Value<bool>("nowplaying");
                t.Rank = attrToken.Value<int?>("rank");
            }

            // api returns milliseconds when track.getInfo is called directly
            var secsStr = token.Value<string>("duration");
            double secs;

            if (double.TryParse(secsStr, out secs))
            {
                if (Math.Abs(secs - default(double)) > double.Epsilon)
                {
                    t.Duration = TimeSpan.FromMilliseconds(secs);
                }
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
        internal static LastTrack ParseJToken(JToken token, string albumName)
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