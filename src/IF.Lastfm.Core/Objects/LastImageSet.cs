using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class LastImageSet : IEnumerable<Uri>
    {
        public LastImageSet()
        {
        }

        public LastImageSet(string s, string m, string l, string xl, string xxl = null)
        {
            Small = new Uri(s);
            Medium = new Uri(m);
            Large = new Uri(l);
            ExtraLarge = new Uri(xl);

            if (xxl != null)
            {
                Mega = new Uri(xxl);
            }
        }

        public Uri Small { get; set; }

        public Uri Medium { get; set; }

        public Uri Large { get; set; }

        public Uri ExtraLarge { get; set; }

        public Uri Mega { get; set; }

        public Uri Largest
        {
            get
            {
                return Mega ?? ExtraLarge ?? Large ?? Medium ?? Small;
            }
        }

        private IEnumerable<Uri> Images
        {
            get
            {
                return new List<Uri>()
                       {
                           Small,
                           Medium,
                           Large,
                           ExtraLarge,
                           Mega
                       };
            }
        }

        public static LastImageSet ParseJToken(JToken images)
        {
            var c = new LastImageSet();

            foreach (var image in images.Children())
            {
                var size = image.Value<string>("size");
                var uriString = image.Value<string>("#text");

                if (string.IsNullOrEmpty(uriString))
                {
                    break;
                }

                var uri = new Uri(uriString, UriKind.Absolute);

                switch (size)
                {
                    case "small":
                        c.Small = uri;
                        break;
                    case "medium":
                        c.Medium = uri;
                        break;
                    case "large":
                        c.Large = uri;
                        break;
                    case "extralarge":
                        c.ExtraLarge = uri;
                        break;
                    case "mega":
                        c.Mega = uri;
                        break;
                }
            }

            return c;
        }

        public IEnumerator<Uri> GetEnumerator()
        {
            return Images.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}