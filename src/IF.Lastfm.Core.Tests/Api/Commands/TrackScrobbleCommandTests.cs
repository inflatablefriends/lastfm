using System;
using System.Collections.Generic;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.Track;
using IF.Lastfm.Core.Api.Helpers;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class TrackScrobbleCommandTests : CommandTestsBase
    {
        private DateTimeOffset _scrobblePlayed;
        private Scrobble _testScrobble;
        private ScrobbleCommand _command;

        public TrackScrobbleCommandTests()
        {
            _scrobblePlayed = DateTimeOffset.UtcNow;
            _testScrobble = new Scrobble("Kate Nash", "Made of Bricks", "Foundations", _scrobblePlayed)
            {
                AlbumArtist = "Kate Nash"
            };

            _command = new ScrobbleCommand(MAuth.Object, _testScrobble);
        }

        [Test]
        public void ParametersCorrect()
        {
            var expected = new Dictionary<string, string>
            {
                {"artist", "Kate Nash"},
                {"albumArtist", "Kate Nash"},
                {"album", "Made of Bricks"},
                {"track", "Foundations"},
                {"chosenByUser", "0"},
                {"timestamp", _scrobblePlayed.AsUnixTime().ToString()}
            };

            _command.SetParameters();

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }
    }
}