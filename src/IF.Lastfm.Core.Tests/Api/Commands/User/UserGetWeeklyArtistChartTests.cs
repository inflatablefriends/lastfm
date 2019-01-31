using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Commands.User;
using NUnit.Framework;
using Moq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetWeeklyArtistChartTests : CommandTestsBase
    {
        private const string user = "test";

        private GetWeeklyArtistChartCommand _command;
        private Mock<ILastAuth> _mockAuth;

        [SetUp]
        public void TestInitialise()
        {
            _mockAuth = new Mock<ILastAuth>();
            _command = new GetWeeklyArtistChartCommand(_mockAuth.Object, user)
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

            GetWeeklyArtistChartCommand _command2 = new GetWeeklyArtistChartCommand(_mockAuth.Object, user)
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
            var file = GetFileContents("UserApi.UserGetWeeklyArtistChart.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);
            var first = parsed.Content.First();
            //General tests
            Assert.IsTrue(parsed.Success);
            Assert.AreEqual(20, parsed.Content.Count);

            //Tests on values being properly set
            Assert.AreEqual("Bing Crosby", first.Name);
            Assert.AreEqual("2437980f-513a-44fc-80f1-b90d9d7fcf8f", first.Mbid);
            Assert.AreEqual(18, first.PlayCount);
            Assert.AreEqual(new Uri("https://www.last.fm/music/Bing+Crosby"), first.Url); 
        }

    }
}