using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    [TestClass]
    public class TrackUpdateNowPlayingCommandTests : CommandIntegrationTestsBase
    {
        private const string ARTIST_NAME = "Crystal Castles";
        private const string ALBUM_NAME = "Crystal Castles ( II )";
        private const string TRACK_NAME = "Not in Love";

        [TestMethod]
        public async Task UpdatesNowPlaying()
        {
            var trackPlayed = DateTime.UtcNow.AddMinutes(-1);
            var testScrobble = new Scrobble(ARTIST_NAME, ALBUM_NAME, TRACK_NAME, trackPlayed)
            {
                Duration = new TimeSpan(0, 0, 3, 49, 200),
                AlbumArtist = ARTIST_NAME
            };

            var trackApi = new TrackApi(Auth);
            var response = await trackApi.UpdateNowPlayingAsync(testScrobble);

            Assert.IsTrue(response.Success);

            var userApi = new UserApi(Auth);
            var tracks = await userApi.GetRecentScrobbles(Auth.UserSession.Username, null, 1, 1);

            var expectedTrack = new LastTrack
            {
                Name = TRACK_NAME,
                ArtistName = ARTIST_NAME,
                AlbumName = ALBUM_NAME,
                Mbid = "1b9ee1d8-c5a7-44d9-813e-85beb0d59f1b",
                ArtistMbid = "b1570544-93ab-4b2b-8398-131735394202",
                Url = new Uri("http://www.last.fm/music/Crystal+Castles/_/Not+in+Love"),
                Images = new LastImageSet("http://userserve-ak.last.fm/serve/34s/61473043.png",
                    "http://userserve-ak.last.fm/serve/64s/61473043.png",
                    "http://userserve-ak.last.fm/serve/126/61473043.png",
                    "http://userserve-ak.last.fm/serve/300x300/61473043.png"),
                IsNowPlaying = true
            };

            var expectedJson = expectedTrack.TestSerialise();
            var actualJson = tracks.Content.FirstOrDefault().TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }
    }
}