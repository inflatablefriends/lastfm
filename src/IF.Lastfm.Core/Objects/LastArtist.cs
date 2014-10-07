using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    /// <summary>
    /// Todo bio, tour, similar, stats, streamable
    /// </summary>
    public class LastArtist : ILastFmObject
    {
        #region Properties

        public string Name { get; set; }
        public string Mbid { get; set; }
        public Uri Url { get; set; }
        public bool OnTour { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

        public LastImageSet MainImage { get; set; }

        #endregion

        internal static LastArtist ParseJToken(JToken token)
        {
            var a = new LastArtist();

            a.Name = token.Value<string>("name");
            a.Mbid = token.Value<string>("mbid");
            a.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            a.OnTour = Convert.ToBoolean(token.Value<int>("ontour"));
            
            var tagsToken = token.SelectToken("tags");
            if (tagsToken != null && tagsToken.HasValues)
            {
                a.Tags = tagsToken.SelectToken("tag").Children().Select(Tag.ParseJToken);
            }

            var images = token.SelectToken("image");
            if (images != null && images.HasValues)
            {
                var imageCollection = LastImageSet.ParseJToken(images);
                a.MainImage = imageCollection;
            }
            
            return a;
        }

        internal static string GetNameFromJToken(JToken artistToken)
        {
            var name = artistToken.Value<string>("name");

            if (string.IsNullOrEmpty(name))
            {
                name = artistToken.Value<string>("#text");
            }

            return name;
        }
    }
}