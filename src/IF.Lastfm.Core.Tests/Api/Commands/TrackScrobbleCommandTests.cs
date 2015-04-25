using System;
using System.Collections.Generic;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.Track;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
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
                {"artist[0]", "Kate Nash"},
                {"albumArtist[0]", "Kate Nash"},
                {"album[0]", "Made of Bricks"},
                {"track[0]", "Foundations"},
                {"chosenByUser[0]", "0"},
                {"timestamp[0]", _scrobblePlayed.AsUnixTime().ToString()}
            };

            _command.SetParameters();

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }
    }
}