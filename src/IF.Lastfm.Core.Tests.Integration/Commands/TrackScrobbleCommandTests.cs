using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Scrobblers;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    /// <summary>
    /// ScrobblerTestsBase is the main place for all things scrobbler testing
    /// But it's useful to have a test actually hitting the API
    /// </summary>
    public class TrackScrobbleCommandTests : CommandIntegrationTestsBase
    {
        private const string ARTIST_NAME = "Hot Chip";
        private const string ALBUM_NAME = "The Warning";
        private const string TRACK_NAME = "Over and Over";

        [Test]
        public async Task ScrobblesSingle()
        {
            var trackPlayed = DateTimeOffset.UtcNow.AddMinutes(-1).RoundToNearestSecond();
            var testScrobble = new Scrobble("Hot Chip", "The Warning", "Over and Over", trackPlayed)
            {
                AlbumArtist = ARTIST_NAME,
                ChosenByUser = false
            };

            var trackApi = new TrackApi(Auth);
            var response = await trackApi.ScrobbleAsync(testScrobble);

            Assert.IsTrue(response.Success);
            
            var testGuid = Guid.Empty;
            var expectedTrack = new LastTrack
            {
                Name = TRACK_NAME,
                ArtistName = ARTIST_NAME,
                AlbumName = ALBUM_NAME,
                Mbid = testGuid.ToString("D"),
                ArtistMbid = testGuid.ToString("D"),
                Url = new Uri("http://www.last.fm/music/Hot+Chip/_/Over+and+Over"),
                Images = new LastImageSet("http://userserve-ak.last.fm/serve/34s/50921593.png",
                    "http://userserve-ak.last.fm/serve/64s/50921593.png",
                    "http://userserve-ak.last.fm/serve/126/50921593.png",
                    "http://userserve-ak.last.fm/serve/300x300/50921593.png")
            };
            var expectedJson = expectedTrack.TestSerialise();

            var userApi = new UserApi(Auth);
            var tracks = await userApi.GetRecentScrobbles(Auth.UserSession.Username, null, 0, 1);
            Assert.IsTrue(tracks.Any());

            var actual = tracks.Content.First();
            
            TestHelper.AssertSerialiseEqual(trackPlayed, actual.TimePlayed);
            actual.TimePlayed = null;

            // MBIDs returned by last.fm change from time to time, so let's just test that they're there.
            Assert.IsTrue(Guid.Parse(actual.Mbid) != Guid.Empty);
            Assert.IsTrue(Guid.Parse(actual.ArtistMbid) != Guid.Empty);
            actual.Mbid = testGuid.ToString("D");
            actual.ArtistMbid = testGuid.ToString("D");

            var actualJson = actual.TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task ScrobblesMultiple()
        {
            var scrobbles = GenerateScrobbles(50);

            var scrobbler = new Scrobbler(Auth);
            var response = await scrobbler.ScrobbleAsync(scrobbles);

            Assert.IsTrue(response.Status == LastResponseStatus.Successful);
            Assert.IsTrue(response.Success);
        }

        private string[] testScrobbles = new[]
        {
            "Hot Chip|Playboy|Coming on Strong",
            "Broken Social Scene|Our Faces Split the Coast in Half|Broken Social Scene",
            "The Knife|Heartbeats|Deep Cuts",
            "The Knife|Pass this on|Deep Cuts",
            "The Knife|Silent Shout|Silent Shout"
        };

        private IList<Scrobble> GenerateScrobbles(int amount)
        {
            var now = DateTimeOffset.UtcNow.AddHours(-5);
            return Enumerable.Range(0, amount).Select(i =>
            {
                var metadata = testScrobbles[i%5].Split('|');
                return new Scrobble(metadata[0], metadata[2], metadata[1], now.AddMinutes(-6));
            }).ToList();
        }
    }
}
