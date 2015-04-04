using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Scrobblers;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scrobbler = IF.Lastfm.Core.Scrobblers.Scrobbler;

namespace IF.Lastfm.Core.Tests.Scrobblers
{
    [TestClass]
    public class ScrobblerTests
    {
        private Mock<ILastAuth> _mockAuth;
        private Scrobbler _scrobbler;
        private QueueFakeResponseHandler _fakeHandler;

        [TestInitialize]
        public void Initialise()
        {
            _mockAuth = new Mock<ILastAuth>();
            _fakeHandler = new QueueFakeResponseHandler();

            var httpClient = new HttpClient(_fakeHandler);
            _scrobbler = new Scrobbler(_mockAuth.Object, httpClient);
        }



        [TestMethod]
        public async Task CorrectResponseWithBadAuth()
        {
            _mockAuth.SetupGet(m => m.Authenticated).Returns(false);

            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.OK, TrackApiResponses.TrackScrobbleSuccess);
            var scrobbleResponse = await ExecuteTestInternal(responseMessage);

            Assert.AreEqual(LastResponseStatus.BadAuth, scrobbleResponse.Status);
        }

        [TestMethod]
        public async Task CorrectResponseWhenRequestSuccessful()
        {
            _mockAuth.SetupGet(m => m.Authenticated).Returns(true);

            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.OK, TrackApiResponses.TrackScrobbleSuccess);
            var scrobbleResponse = await ExecuteTestInternal(responseMessage);

            Assert.AreEqual(LastResponseStatus.Successful, scrobbleResponse.Status);
        }

        [TestMethod]
        public async Task CorrectResponseWhenRequestFailed()
        {
            _mockAuth.SetupGet(m => m.Authenticated).Returns(true);

            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.RequestTimeout, new byte[0]);
            var scrobbleResponse = await ExecuteTestInternal(responseMessage);

            Assert.AreEqual(LastResponseStatus.RequestFailed, scrobbleResponse.Status);
        }


        private async Task<ScrobbleResponse> ExecuteTestInternal(HttpResponseMessage responseMessage)
        {
            var testScrobble = new Scrobble("65daysofstatic", "The Fall of Math", "Hole", ApiExtensions.FromUnixTime(1428175531))
            {
                ChosenByUser = true
            };

            _fakeHandler.Enqueue(responseMessage);

            var scrobbleResponse = await _scrobbler.ScrobbleAsync(testScrobble);
            return scrobbleResponse;
        }
    }
}
