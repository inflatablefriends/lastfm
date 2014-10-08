using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class Album : ILastFmObject
    {
        #region Properties

        public string Name { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
        
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
        internal static Album ParseJToken(JToken token)
        {
            var a = new Album();

            var artistToken = token["artist"];
            if (artistToken.Type == JTokenType.String)
            {
                a.ArtistName = token.Value<string>("artist");
                a.ArtistId = token.Value<string>("id");
            }
            else if (artistToken.Type == JTokenType.Object)
            {
                a.ArtistName = artistToken.Value<string>("name");
                a.ArtistId = artistToken.Value<string>("mbid");
            }

            var tracksToken = token.SelectToken("tracks");
            if (tracksToken != null)
            {
                var trackToken = tracksToken.SelectToken("track");
                if (trackToken != null)
                {
                    a.Tracks = tracksToken.Children().Select(t => Track.ParseJToken(t, a.Name));
                }
            }

            var tagsToken = token.SelectToken("toptags");
            if (tagsToken != null)
            {
                var tagToken = tagsToken.SelectToken("tag");
                a.TopTags = tagToken.Children().Select(Tag.ParseJToken);
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
