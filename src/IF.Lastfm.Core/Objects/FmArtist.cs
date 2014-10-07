﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    /// <summary>
    /// Todo bio, tour, similar, stats, streamable
    /// </summary>
    public class FmArtist : ILastFmObject
    {
        #region Properties

        public string Name { get; set; }
        public string Mbid { get; set; }
        public Uri Url { get; set; }
        public bool OnTour { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public LastImageCollection Images { get; set; }

        #endregion

        internal static FmArtist ParseJToken(JToken token)
        {
            var a = new FmArtist();

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
                var imageCollection = LastImageCollection.ParseJToken(images);
                a.Images = imageCollection;
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