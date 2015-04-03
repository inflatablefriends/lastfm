using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.UserApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    [TestClass]
    public class UserGetTopAlbumsCommandTests : CommandTestsBase
    {
        private const string USER = "test";
        private const LastStatsTimeSpan SPAN = LastStatsTimeSpan.Month;

        private GetTopAlbumsCommand _command;
        private Mock<ILastAuth> _mockAuth;

        [TestInitialize]
        public void TestInitialise()
        {
            _mockAuth = new Mock<ILastAuth>();
            _command = new GetTopAlbumsCommand(_mockAuth.Object, USER, SPAN)
            {
                Page = 5,
                Count = 20
            };

            _command.SetParameters();
        }

        [TestMethod]
        public void CorrectParameters()
        {
            var expected = new Dictionary<string, string>
            {
                {"user", USER},
                {"period", SPAN.GetApiName()},
                {"limit", "20"},
                {"page", "5"},
                {"disablecachetoken", ""}
            };

            _command.Parameters["disablecachetoken"] = "";

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }

        [TestMethod]
        public async Task HandleErrorResponse()
        {
            var http = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopAlbumsError));
            var response = await _command.HandleResponse(http);

            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task HandleResponseEmpty()
        {
            var http = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopAlbumsEmpty));
            var response = await _command.HandleResponse(http);

            Assert.IsTrue(response.Success);
            Assert.IsTrue(!response.Content.Any());
            Assert.AreEqual(0, response.TotalItems);
            Assert.AreEqual(1, response.TotalPages);
        }

        [TestMethod]
        public async Task HandleResponseSingle()
        {
            var expectedAlbum = new LastAlbum
            {
                ArtistName = "Crystal Castles",
                Name = "Crystal Castles",
                PlayCount = 2206,
                Mbid = "a432a420-f374-4556-8421-b4ea097c7fe9",
                Url = new Uri("http://www.last.fm/music/Crystal+Castles/Crystal+Castles"),
                ListenerCount = null,
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/78606386.png",
                    "http://userserve-ak.last.fm/serve/64s/78606386.png",
                    "http://userserve-ak.last.fm/serve/126/78606386.png",
                    "http://userserve-ak.last.fm/serve/300x300/78606386.png"),
                Tracks = Enumerable.Empty<LastTrack>(),
                ReleaseDateUtc = null,
                TopTags = Enumerable.Empty<LastTag>()
            };

            var http = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopAlbumsSingle));
            var response = await _command.HandleResponse(http);
            
            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Content.Count);

            var actualAlbum = response.Content.First();
            
            TestHelper.AssertSerialiseEqual(expectedAlbum, actualAlbum);
        }

        [TestMethod]
        public async Task HandleResponseMultiple()
        {

        }
    }
}