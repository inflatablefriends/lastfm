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
using Newtonsoft.Json;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetWeeklyAlbumChartTests : CommandTestsBase
    {
        private const string user = "test";

        private GetWeeklyAlbumChartCommand _command;
        private Mock<ILastAuth> _mockAuth;

        [SetUp]
        public void TestInitialise()
        {
            _mockAuth = new Mock<ILastAuth>();
            _command = new GetWeeklyAlbumChartCommand(_mockAuth.Object, user)
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

            GetWeeklyAlbumChartCommand _command2 = new GetWeeklyAlbumChartCommand(_mockAuth.Object, user)
            {
                //no parameters
            };

            _command2.SetParameters();
            _command2.Parameters["disablecachetoken"] = "";

            TestHelper.AssertSerialiseEqual(expected, _command2.Parameters);
        }

        [Test]
        public async Task GetWeeklyAlbumChart_Empty_Success()
        {
            var file = GetFileContents("UserApi.UserGetWeeklyAlbumChartEmpty.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsTrue(parsed.Content.Count == 0);
        }

        [Test]
        public async Task GetWeeklyAlbumChart_HandleResponse_Success()
        {
            var file = GetFileContents("UserApi.UserGetWeeklyAlbumChart.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);
            var first = parsed.Content.First();

            var expectedFirst = new LastAlbum() {
                ArtistName =  "Bing Crosby",
                ArtistMbid = "2437980f-513a-44fc-80f1-b90d9d7fcf8f",
                Name = "Bing Crosby - Christmas Classics",
                Mbid = "2e427dea-3565-4ba6-963b-2b2e271b0d53",
                Tracks = new List<LastTrack>{},
                TopTags = new List<LastTag>{},
                PlayCount = 8,
                Url = new Uri("https://www.last.fm/music/Bing+Crosby/Bing+Crosby+-+Christmas+Classics"),
            };

            //General tests
            Assert.IsTrue(parsed.Success);
            Assert.AreEqual(175, parsed.Content.Count);

            //Test values of first album
            var expectedJson = JsonConvert.SerializeObject(expectedFirst, Formatting.Indented);
            var actualJson = JsonConvert.SerializeObject(first, Formatting.Indented);

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
            
            //Tests on attribute values
            Assert.AreEqual((double)1545566400, parsed.From);
            Assert.AreEqual((double)1546171200, parsed.To);
        }

    }
}