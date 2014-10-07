using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class LastAlbum : ILastFmObject
    {
        #region Properties

        public string Name { get; set; }
        public IEnumerable<LastTrack> Tracks { get; set; }
        
        public string ArtistName { get; set; }
        public string ArtistId { get; set; }
        
        public DateTime ReleaseDateUtc { get; set; }

        public int ListenerCount { get; set; }
        public int TotalPlayCount { get; set; }

        public string Mbid { get; set; }

        public IEnumerable<Tag> TopTags { get; set; }

        public Uri Url { get; set; }

        public LastImageSet Images { get; set; }
        
        #endregion

        /// <summary>
        /// TODO datetime parsing
        /// TODO images
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static LastAlbum ParseJToken(JToken token)
        {
            var a = new LastAlbum();

            try
            {
                a.ArtistName = token.Value<string>("artist");
                a.ArtistId = token.Value<string>("id");

                var tracksToken = token.SelectToken("tracks").SelectToken("track");
                if (tracksToken != null)
                {
                    a.Tracks = tracksToken.Children().Select(trackToken => LastTrack.ParseJToken(trackToken, a.Name));
                }

                var tagsToken = token.SelectToken("toptags").SelectToken("tag");
                if (tagsToken != null)
                {
                    a.TopTags = tagsToken.Children().Select(Tag.ParseJToken);
                }
            }
            catch
            {
                // for when artist is not a string but a LastArtist object
                var artist = token.SelectToken("artist").ToObject<LastArtist>();
                a.ArtistName = artist.Name;
                a.ArtistId = artist.Mbid;
            }

            a.ListenerCount = token.Value<int>("listeners");
            a.Mbid = token.Value<string>("mbid");
            a.Name = token.Value<string>("name");
            a.TotalPlayCount = token.Value<int>("playcount");

            var images = token.SelectToken("image");
            if (images != null)
            {
                var imageCollection = LastImageSet.ParseJToken(images);
                a.Images = imageCollection;
            }
            
            a.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            return a;
        }

        internal static string GetNameFromJToken(JToken albumToken)
        {
            var name = albumToken.Value<string>("name");

            if (string.IsNullOrEmpty(name))
            {
                name = albumToken.Value<string>("#text");
            }

            return name;
        }
    }
}
