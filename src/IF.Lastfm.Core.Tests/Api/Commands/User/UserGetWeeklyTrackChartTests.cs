using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Commands.User;
using NUnit.Framework;
using Moq;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetWeeklyTrackChartTests : CommandTestsBase
    {
        private const string user = "test";

        private GetWeeklyTrackChartCommand _command;
        private Mock<ILastAuth> _mockAuth;

        [SetUp]
        public void TestInitialise()
        {
            _mockAuth = new Mock<ILastAuth>();
            _command = new GetWeeklyTrackChartCommand(_mockAuth.Object, user)
            {
                From = 1234,
                To = 5678
             };
            _command.SetParameters();
        }

        [Test]
        public void CorrectParametersWithToFrom()
        {
            var expected = new Dictionary<string, string>
            {
                {"user", user},
                {"from", "1234"},
                {"to", "5678"},
                {"disablecachetoken", ""}
            };
            _command.Parameters["disablecachetoken"] = "";

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }

         [Test]
        public void CorrectParametersNoToFrom()
        {
            var expected = new Dictionary<string, string>
            {
                {"user", user},
                {"disablecachetoken", ""}
            };

            GetWeeklyTrackChartCommand _command2 = new GetWeeklyTrackChartCommand(_mockAuth.Object, user)
            {
                //no parameters
            };

            _command2.SetParameters();
            _command2.Parameters["disablecachetoken"] = "";

            TestHelper.AssertSerialiseEqual(expected, _command2.Parameters);
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("UserApi.UserGetTopAlbumsError.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.AreEqual(LastResponseStatus.MissingParameters, parsed.Status);
        }

        [Test]
        public async Task GetWeeklyArtistChart_HandleResponse_Success()
        {
            var file = GetFileContents("UserApi.UserGetWeeklyTrackChart.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);
            var first = parsed.Content.First();

            LastTrack expected_first = new LastTrack() {

            };
            //General tests
            Assert.IsTrue(parsed.Success);
            Assert.AreEqual(90, parsed.Content.Count);

            //Tests on values being properly set
            Assert.AreEqual("dcb03ce3-67a5-4eb3-b2d1-2a12d93a38f3", first.ArtistMbid);
            Assert.AreEqual("1d552949-8c0b-411a-92c4-1ab34be9a536", first.Mbid);
            Assert.AreEqual(3, first.PlayCount);
            Assert.AreEqual(new Uri("https://www.last.fm/music/B.B.+King/_/The+Thrill+Is+Gone"), first.Url);
            Assert.AreEqual(1, first.Rank);
        }

    }
}