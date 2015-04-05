using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Scrobblers;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IF.Lastfm.Core.Tests.Scrobblers
{
    [TestClass]
    public abstract class ScrobblerTestsBase
    {
        protected IScrobbler Scrobbler { get; private set; }

        protected Mock<ILastAuth> MockAuth { get; private set; }

        protected QueueFakeResponseHandler FakeResponseHandler { get; private set; }

        [TestInitialize]
        public void Initialise()
        {
            MockAuth = new Mock<ILastAuth>();
            FakeResponseHandler = new QueueFakeResponseHandler();
            Scrobbler = GetScrobbler();
        }

        protected async Task<ScrobbleResponse> ExecuteTestInternal(HttpResponseMessage responseMessage)
        {
            var testScrobble = new Scrobble("65daysofstatic", "The Fall of Math", "Hole", ApiExtensions.FromUnixTime(1428175531))
            {
                ChosenByUser = true
            };

            FakeResponseHandler.Enqueue(responseMessage);

            var scrobbleResponse = await Scrobbler.ScrobbleAsync(testScrobble);
            return scrobbleResponse;
        }

        protected abstract IScrobbler GetScrobbler();


        [TestMethod]
        public async Task CorrectResponseWithBadAuth()
        {
            MockAuth.SetupGet(m => m.Authenticated).Returns(false);

            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.OK, TrackApiResponses.TrackScrobbleSuccess);
            var scrobbleResponse = await ExecuteTestInternal(responseMessage);

            Assert.AreEqual(LastResponseStatus.BadAuth, scrobbleResponse.Status);
        }

        [TestMethod]
        public async Task CorrectResponseWhenRequestSuccessful()
        {
            MockAuth.SetupGet(m => m.Authenticated).Returns(true);

            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.OK, TrackApiResponses.TrackScrobbleSuccess);
            var scrobbleResponse = await ExecuteTestInternal(responseMessage);

            Assert.AreEqual(LastResponseStatus.Successful, scrobbleResponse.Status);
        }

        [TestMethod]
        public async Task CorrectResponseWhenRequestFailed()
        {
            MockAuth.SetupGet(m => m.Authenticated).Returns(true);

            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.RequestTimeout, new byte[0]);
            var scrobbleResponse = await ExecuteTestInternal(responseMessage);

            Assert.AreEqual(LastResponseStatus.RequestFailed, scrobbleResponse.Status);
        }
    }
}