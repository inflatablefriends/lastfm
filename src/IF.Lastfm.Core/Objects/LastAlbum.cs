using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class LastAlbum : ILastfmObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<LastTrack> Tracks { get; set; }
        
        public string ArtistName { get; set; }
        
        public DateTimeOffset? ReleaseDateUtc { get; set; }

        public int? ListenerCount { get; set; }

        public int? PlayCount { get; set; }

        public int? UserPlayCount { get; set; }

        public string Mbid { get; set; }

        public IEnumerable<LastTag> TopTags { get; set; }

        public Uri Url { get; set; }

        public LastImageSet Images { get; set; }
        
        internal static LastAlbum ParseJToken(JToken token)
        {
            var a = new LastAlbum();

            a.Id = token.Value<string>("id");
            var artistToken = token["artist"];
            switch (artistToken.Type)
            {
                case JTokenType.String:
                    a.ArtistName = token.Value<string>("artist");
                    break;
                case JTokenType.Object:
                    a.ArtistName = artistToken.Value<string>("name");
                    break;
            }

            var tracksToken = token.SelectToken("tracks");
            if (tracksToken != null)
            {
                var trackToken = tracksToken.SelectToken("track");
                if (trackToken != null)
                    a.Tracks = trackToken.Type == JTokenType.Array
                        ? trackToken.Children().Select(t => LastTrack.ParseJToken(t, a.Name))
                        : new List<LastTrack>() {LastTrack.ParseJToken(trackToken, a.Name)};
            }
            else
            {
                a.Tracks = Enumerable.Empty<LastTrack>();
            }

            var tagsToken = token.SelectToken("toptags");
            if (tagsToken != null)
            {
                var tagToken = tagsToken.SelectToken("tag");
                if (tagToken != null)
                {
                    a.TopTags = 
                        tagToken.Type == JTokenType.Array 
                        ? tagToken.Children().Select(token1 => LastTag.ParseJToken(token1)) 
                        : new List<LastTag> { LastTag.ParseJToken(tagToken) };
                }
            }
            else
            {
                a.TopTags = Enumerable.Empty<LastTag>();
            }
    
            a.ListenerCount = token.Value<int?>("listeners");
            a.Mbid = token.Value<string>("mbid");
            a.Name = token.Value<string>("name");

            var playCountStr = token.Value<string>("playcount");
            int playCount;
            if (int.TryParse(playCountStr, out playCount))
            {
                a.PlayCount = playCount;
            }

            var userPlayCountStr = token.Value<string>("userplaycount");
            int userPlayCount;
            if (int.TryParse(userPlayCountStr, out userPlayCount))
            {
                a.UserPlayCount = userPlayCount;
            }

            var images = token.SelectToken("image");
            if (images != null)
            {
                var imageCollection = LastImageSet.ParseJToken(images);
                a.Images = imageCollection;
            }
            
            a.Url = new Uri(token.Value<string>("url"), UriKind.Absolute);

            var dateString = token.Value<string>("releasedate");
            DateTimeOffset releaseDate;
            if (DateTimeOffset.TryParse(dateString, out releaseDate))
            {
                a.ReleaseDateUtc = releaseDate;
            }

            return a;
        }

        internal static string GetNameFromJToken(JToken albumToken)
        {
            var name = albumToken.Value<string>("title")
                       ?? albumToken.Value<string>("#text")
                       ?? albumToken.Value<string>("name"); // Used in Library track lists
            
            return name;
        }
    }
}
