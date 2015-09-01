using System;
using System.Collections.Generic;
using System.Linq;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.Track;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Scrobblers;
using IF.Lastfm.Core.Tests.Resources;

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

        [Test]
        public async Task HandlesRejectedResponse()
        {
            var responseMessage = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackScrobbleRejected));
            var response = await _command.HandleResponse(responseMessage) as ScrobbleResponse;
            
            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Ignored.Count());
            Assert.AreEqual("Artist name failed filter: Various", response.Ignored.First().IgnoredReason);
        }

        [Test]
        public async Task HandlesSuccessResponse()
        {
            var responseMessage = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackScrobbleSuccess));
            var response = await _command.HandleResponse(responseMessage);
            
            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.AcceptedCount);
        }

        [Test]
        public async Task HandlesSuccessResponseWhenMissingAlbumProperty()
        {
            var responseMessage = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackScrobbleSuccessNoAlbumProperty));
            var response = await _command.HandleResponse(responseMessage);
            
            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.AcceptedCount);
        }
    }
}
