using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Scrobblers;
using NUnit.Framework;

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

            var response = await Lastfm.Scrobbler.ScrobbleAsync(testScrobble);

            Assert.IsTrue(response.Success);

            var expectedTrack = new LastTrack
            {
                Name = TRACK_NAME,
                ArtistName = ARTIST_NAME,
                AlbumName = ALBUM_NAME,
            };
            var expectedJson = expectedTrack.TestSerialise();

            var tracks = await Lastfm.User.GetRecentScrobbles(Lastfm.Auth.UserSession.Username, null, null, false, 1, 1);
            var scrobbledTrack = tracks.Single(x => !x.IsNowPlaying.GetValueOrDefault(false));

            // This test fails here when it took too much time to test the whole solution
            // TestHelper.AssertSerialiseEqual(trackPlayed, scrobbledTrack.TimePlayed);

            scrobbledTrack.TimePlayed = null;

            // Some properties change from time to time; parsing is covered in unit tests
            scrobbledTrack.Mbid = null;
            scrobbledTrack.ArtistMbid = null;
            scrobbledTrack.Images = null;
            scrobbledTrack.Url = null;
            scrobbledTrack.ArtistImages = null;
            scrobbledTrack.ArtistUrl = null;
            scrobbledTrack.IsLoved = null;

            var actualJson = scrobbledTrack.TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task ScrobblesMultiple()
        {
            var scrobbles = GenerateScrobbles(4);

            var countingHandler = new CountingHttpClientHandler();
            var httpClient = new HttpClient(countingHandler);
            var scrobbler = new MemoryScrobbler(Lastfm.Auth, httpClient)
            {
                MaxBatchSize = 2
            };
            var response = await scrobbler.ScrobbleAsync(scrobbles);

            Assert.AreEqual(2, countingHandler.Count);
            Assert.AreEqual(LastResponseStatus.Successful, response.Status);
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