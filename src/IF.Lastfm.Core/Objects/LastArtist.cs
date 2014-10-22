using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    /// <summary>
    /// Todo bio, tour, similar, stats, streamable
    /// </summary>
    public class LastArtist : ILastfmObject
    {
        #region Properties

        public string Id { get; set; }
        public string Name { get; set; }
        public string Mbid { get; set; }
        public Uri Url { get; set; }
        public bool OnTour { get; set; }
        public IEnumerable<LastTag> Tags { get; set; }

        public LastImageSet MainImage { get; set; }

        #endregion

        internal static LastArtist ParseJToken(JToken token)
        {
            var a = new LastArtist();

            a.Id = token.Value<string>("id");
            a.Name = token.Value<string>("name");
            a.Mbid = token.Value<string>("mbid");
            var url = token.Value<string>("url");

            // for some stupid reason the api returns the url without http in the get similar method, WHY?
            if (!url.StartsWith("http"))
                url = "http://" + url;

            a.Url = new Uri(url, UriKind.Absolute);

            a.OnTour = Convert.ToBoolean(token.Value<int>("ontour"));
            
            var tagsToken = token.SelectToken("tags");
            if (tagsToken != null)
            {
                var tagToken = tagsToken.SelectToken("tag");
                if (tagToken != null)
                {
                    a.Tags =
                        tagToken.Type == JTokenType.Array
                        ? tagToken.Children().Select(LastTag.ParseJToken)
                        : new List<LastTag> { LastTag.ParseJToken(tagToken) };
                }
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