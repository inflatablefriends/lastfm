using System;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using IF.Lastfm.Core.Tests.Resources;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace IF.Lastfm.Core.Tests.Objects
{
    [TestClass]
    public class LastTrackTests : ObjectsTestsBase
    {
        [TestMethod]
        public async Task TestMethod1()
        {
           // var json = await response.Content.ReadAsStringAsync();
            var response = CreateResponseMessage(Encoding.UTF8.GetString(LibraryApiResponses.LibraryGetTracksMultiple));

            var json = await response.Content.ReadAsStringAsync();

            JToken jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("tracks");

   

            var tracksToken = jtoken.SelectToken("track");

            
            var tracks = new List<LastTrack>();
            foreach (var track in tracksToken.Children())
            {
                var t = LastTrack.ParseJToken(track);

                tracks.Add(t);
            }

            Assert.AreEqual(tracks.Count, 20);
            
         //   var actual = parsed.Content;
            //var token = JsonConvert.DeserializeObject<JToken>(json);//.SelectToken("shouts"); ;
            //LastTrack.ParseJToken(jtoken);
        }
    }
}
