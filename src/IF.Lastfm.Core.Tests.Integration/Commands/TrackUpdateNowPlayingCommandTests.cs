using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    
    public class TrackUpdateNowPlayingCommandTests : CommandIntegrationTestsBase
    {
        private const string ARTIST_NAME = "Crystal Castles";
        private const string ALBUM_NAME = "Crystal Castles ( II )";
        private const string TRACK_NAME = "Not in Love";

        [Test]
        public async Task UpdatesNowPlaying()
        {
            var trackPlayed = DateTime.UtcNow.AddMinutes(-1);
            var testScrobble = new Scrobble(ARTIST_NAME, ALBUM_NAME, TRACK_NAME, trackPlayed)
            {
                Duration = new TimeSpan(0, 0, 3, 49, 200),
                AlbumArtist = ARTIST_NAME
            };

            var response = await Lastfm.Track.UpdateNowPlayingAsync(testScrobble);

            Assert.IsTrue(response.Success);

            await Task.Delay(1000);

            var tracks = await Lastfm.User.GetRecentScrobbles(Lastfm.Auth.UserSession.Username, null, 1, 1);

            var expectedTrack = new LastTrack
            {
                Name = TRACK_NAME,
                ArtistName = ARTIST_NAME,
                AlbumName = ALBUM_NAME,
                IsNowPlaying = true
            };

            var actual = tracks.Content.Single(x => x.IsNowPlaying.GetValueOrDefault(false));

            // Some properties change from time to time
            actual.Mbid = null;
            actual.ArtistMbid = null;
            actual.Images = null;
            actual.Url = null;

            var expectedJson = expectedTrack.TestSerialise();
            var actualJson = actual.TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }
    }
}