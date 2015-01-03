using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    [TestClass]
    public class TrackScrobbleCommandTests : CommandIntegrationTestsBase
    {
        private const string ARTIST_NAME = "Hot Chip";
        private const string ALBUM_NAME = "The Warning";
        private const string TRACK_NAME = "Over and Over";

        [TestMethod]
        public async Task ScrobblesSingle()
        {
            var trackPlayed = DateTime.UtcNow.AddMinutes(-1);
            var testScrobble = new Scrobble("Hot Chip", "The Warning", "Over and Over")
            {
                AlbumArtist = ARTIST_NAME,
                TimePlayed = trackPlayed,
                ChosenByUser = false
            };

            var trackApi = new TrackApi(Auth);
            var response = await trackApi.ScrobbleAsync(testScrobble);

            Assert.IsTrue(response.Success);

            var userApi = new UserApi(Auth);
            var tracks = await userApi.GetRecentScrobbles(Auth.UserSession.Username, null, 0, 1);

            var expectedTrack = new LastTrack
            {
                Name = TRACK_NAME,
                ArtistName = ARTIST_NAME,
                AlbumName = ALBUM_NAME,
                Mbid = "c1af4137-92c5-43e4-ba4a-b43c7004a624",
                ArtistMbid = "d8915e13-d67a-4aa0-9c0b-1f126af951af",
                Url = new Uri("http://www.last.fm/music/Hot+Chip/_/Over+and+Over"),
                Images = new LastImageSet("http://userserve-ak.last.fm/serve/34s/50921593.png",
                    "http://userserve-ak.last.fm/serve/64s/50921593.png",
                    "http://userserve-ak.last.fm/serve/126/50921593.png",
                    "http://userserve-ak.last.fm/serve/300x300/50921593.png"),
                TimePlayed = trackPlayed.RoundToNearestSecond()
            };

            var expectedJson = expectedTrack.TestSerialise();
            var actualJson = tracks.Content.FirstOrDefault().TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }
    }
}
