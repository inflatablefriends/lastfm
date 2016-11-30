using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    /// <summary>
    /// TODO streamable
    /// "streamable": "0"
    /// 
    /// TODO band members
    /// "bandmembers": {
    ///   "member": [
    ///     {
    ///       "name": "Scott Hutchison",
    ///       "yearfrom": "2003"
    ///     },
    ///     {
    ///       "name": "Billy Kennedy",
    ///       "yearfrom": "2006"
    ///     },
    ///     {
    ///       "name": "Grant Hutchison",
    ///       "yearfrom": "2004"
    ///     },
    ///     {
    ///       "name": "Andy Monaghan",
    ///       "yearfrom": "2008"
    ///     },
    ///     {
    ///       "name": "Gordon Skene",
    ///       "yearfrom": "2009"
    ///     }
    ///   ]
    /// }
    /// 
    /// TODO context -> similar, rename similar to related
    /// </summary>
    public class LastArtist : ILastfmObject
    {
        #region Properties

        public string Id { get; set; }
        public string Name { get; set; }
        public LastWiki Bio { get; set; }
        public string Mbid { get; set; }
        public Uri Url { get; set; }
        public bool OnTour { get; set; }
        public IEnumerable<LastTag> Tags { get; set; }
        public List<LastArtist> Similar { get; set; }
        public LastImageSet MainImage { get; set; }
        public int? PlayCount { get; set; }
        public LastStats Stats { get; set; }

        #endregion

        public LastArtist()
        {
            Tags = Enumerable.Empty<LastTag>();
            Similar = Enumerable.Empty<LastArtist>().ToList();
        }

        internal static LastArtist ParseJToken(JToken token)
        {
            var a = new LastArtist();

            a.Id = token.Value<string>("id");
            a.Name = token.Value<string>("name");
            a.Mbid = token.Value<string>("mbid");
            var url = token.Value<string>("url");

            var playCountStr = token.Value<string>("playcount");
            int playCount;
            if (int.TryParse(playCountStr, out playCount))
            {
                a.PlayCount = playCount;
            }

            // for some stupid reason the api returns the url without http in the get similar method, WHY?
            if (!url.StartsWith("http"))
                url = "http://" + url;

            a.Url = new Uri(url, UriKind.Absolute);

            a.OnTour = Convert.ToBoolean(token.Value<int>("ontour"));

            var statsToken = token.SelectToken("stats");
            if (statsToken != null)
            {
                a.Stats = LastStats.ParseJToken(statsToken);
            }

            var bioToken = token.SelectToken("bio");
            if (bioToken != null)
            {
                a.Bio = LastWiki.ParseJToken(bioToken);
            }

            var tagsToken = token.SelectToken("tags");
            if (tagsToken != null)
            {
                var tagToken = tagsToken.SelectToken("tag");
                if (tagToken != null)
                {
                    a.Tags =
                        tagToken.Type == JTokenType.Array
                        ? tagToken.Children().Select(token1 => LastTag.ParseJToken(token1))
                        : new List<LastTag> { LastTag.ParseJToken(tagToken) };
                }
            }

            var images = token.SelectToken("image");
            if (images != null && images.HasValues)
            {
                var imageCollection = LastImageSet.ParseJToken(images);
                a.MainImage = imageCollection;
            }

            var similarToken = token.SelectToken("similar");
            if (similarToken != null)
            {
                a.Similar = new List<LastArtist>();
                var similarArtists = similarToken.SelectToken("artist");
                if (similarArtists != null && similarArtists.Children().Any())
                {
                    // array notation isn't used on the api when only one object is available
                    if (similarArtists.Type != JTokenType.Array)
                    {
                        var item = ParseJToken(similarArtists);
                        a.Similar.Add(item);
                    }
                    else
                    {
                        var items = similarArtists.Children().Select(ParseJToken);
                        a.Similar.AddRange(items);
                    }
                }
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