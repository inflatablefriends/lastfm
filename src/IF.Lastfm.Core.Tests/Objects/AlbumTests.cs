using System;
using System.Collections.Generic;
using System.Text;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Tests.Objects
{
    [TestClass]
    public class AlbumTests
    {
        [TestMethod]
        public void AlbumParsesValidJson()
        {
            var jo = ResourceManager.LoadResource(Encoding.UTF8.GetString(TestData.AlbumGetInfo));
            
            var parsed = FmAlbum.ParseJToken(jo.SelectToken("album"));

            var expected = new FmAlbum
                {
                    ArtistId = "283786832",
                    ArtistName = "Grimes",
                    ListenerCount = 293542,
                    TotalPlayCount = 10540575,
                    Mbid = "2fd00edb-391a-41ec-8f2f-01e2c202d9eb",
                    Name = "Visions",
                    ReleaseDateUtc = new DateTime(2012, 02, 21, 0, 0, 0),
                    Url = new Uri("http://www.last.fm/music/Grimes/Visions", UriKind.Absolute),
                    TopTags = new List<Tag>
                        {
                            new Tag
                                {
                                    Name = "2012",
                                    Url = new Uri("http://www.last.fm/tag/2012", UriKind.Absolute)
                                },
                            new Tag
                                {
                                    Name = "best of 2012",
                                    Url = new Uri("http://www.last.fm/tag/best%20of%202012", UriKind.Absolute)
                                }
                        },
                    Tracks = new List<FmTrack>
                        {
                        }
                };

            Assert.AreEqual(parsed, expected);
        }
    }
}