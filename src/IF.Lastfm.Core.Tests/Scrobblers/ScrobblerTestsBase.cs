using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Scrobblers;
using IF.Lastfm.Core.Tests.Resources;
using Moq;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Scrobblers
{
    public abstract class ScrobblerTestsBase
    {
        protected ScrobblerBase Scrobbler { get; private set; }

        protected Mock<ILastAuth> MockAuth { get; private set; }

        protected QueueFakeResponseHandler FakeResponseHandler { get; private set; }
        
        protected abstract ScrobblerBase GetScrobbler();

        private List<Scrobble> GetTestScrobbles()
        {
            var counter = 0;
            var now = new DateTimeOffset(2012, 02, 29, 15, 40, 03, TimeSpan.Zero);
            Func<DateTimeOffset> getTimePlayed = () => now.AddSeconds(-360 * counter++);

            var testScrobbles = new List<Scrobble>
            {
                new Scrobble("65daysofstatic", "The Fall of Math", "Hole", getTimePlayed())
                {
                    ChosenByUser = true
                },
                new Scrobble("やくしまるえつこ", "X次元へようこそ", "X次元へようこそ", getTimePlayed())
                {
                    AlbumArtist = "やくしまるえつこ",
                    ChosenByUser = false
                },
                new Scrobble("Björk", "Hyperballad", "Post", getTimePlayed())
                {
                    AlbumArtist = "Björk",
                    ChosenByUser = false
                },
                new Scrobble("Broken Social Scene", "Sentimental X's", "Forgiveness Rock Record", getTimePlayed())
                {
                    AlbumArtist = "Broken Social Scene",
                    ChosenByUser = false
                },
                new Scrobble("Rubies", "Stand in a Line", "Teppan-Yaki (A Collection of Remixes)", getTimePlayed())
                {
                    AlbumArtist = "Schinichi Osawa",
                    ChosenByUser = false
                }
            };

            return testScrobbles;
        }

        private HttpRequestMessage GenerateExpectedRequestMessage(string messageBody)
        {
            var parameters = messageBody.Split('&')
                    .Select(pair => pair.Split('='))
                    .Select(arr => new KeyValuePair<string, string>(arr[0], arr[1]));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, LastFm.ApiRootSsl)
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            return requestMessage;
        }

        [SetUp]
        public virtual void Initialise()
        {
            var testApiKey = "59dd1140db864fd4a68ca820709eaf98";
            var testApiSecret = "fa45357dcd914671a22def63cbe79a46";
            var testUserSession = new LastUserSession
            {
                IsSubscriber = false,
                Token = "071a119a9aac4942b1b05328a5591f55",
                Username = "demo_user"
            };

            MockAuth = new Mock<ILastAuth>();
            MockAuth.SetupGet(m => m.Authenticated).Returns(true);
            MockAuth.SetupGet(m => m.ApiKey).Returns(testApiKey);

            var stubAuth = new LastAuth(testApiKey, testApiSecret);
            stubAuth.LoadSession(testUserSession);
            MockAuth.Setup(m => m.GenerateMethodSignature(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns<string, Dictionary<string, string>>((method, parameters) => stubAuth.GenerateMethodSignature(method, parameters));

            FakeResponseHandler = new QueueFakeResponseHandler();
            Scrobbler = GetScrobbler();
        }

        [TearDown]
        public virtual void Cleanup()
        {
            Scrobbler.Dispose();
        }

        protected async Task<ScrobbleResponse> ExecuteTestInternal(IEnumerable<Scrobble> testScrobbles, HttpResponseMessage responseMessage, HttpRequestMessage expectedRequestMessage = null)
        {
            FakeResponseHandler.Enqueue(responseMessage);

            var scrobbleResponse = await Scrobbler.ScrobbleAsync(testScrobbles);

            if (expectedRequestMessage != null)
            {
                var actualRequestMessage = FakeResponseHandler.Sent.First();
                TestHelper.AssertSerialiseEqual(expectedRequestMessage.Headers, actualRequestMessage.Item1.Headers);
                TestHelper.AssertSerialiseEqual(expectedRequestMessage.RequestUri, actualRequestMessage.Item1.RequestUri);

                var expectedRequestBody = await expectedRequestMessage.Content.ReadAsStringAsync();
                var actualRequestBody = actualRequestMessage.Item2;
                TestHelper.AssertSerialiseEqual(expectedRequestBody, actualRequestBody);
            }

            FakeResponseHandler.Sent.Clear();

            return scrobbleResponse;
        }

        [Test]
        public async Task ScrobbleSingleSuccessful()
        {
            var requestMessage = GenerateExpectedRequestMessage(TrackApiResponses.TrackScrobbleSingleRequestBody);

            var testScrobbles = GetTestScrobbles().Take(1);
            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.OK, TrackApiResponses.TrackScrobbleSuccess);
            var scrobbleResponse = await ExecuteTestInternal(testScrobbles, responseMessage, requestMessage);

            Assert.AreEqual(LastResponseStatus.Successful, scrobbleResponse.Status);
        }

        [Test]
        public async Task ScrobbleMultipleSuccessful()
        {
            var requestMessage = GenerateExpectedRequestMessage(TrackApiResponses.TrackScrobbleMultipleRequestBody);

            var testScrobbles = GetTestScrobbles();
            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.OK, TrackApiResponses.TrackScrobbleSuccess);
            var scrobbleResponse = await ExecuteTestInternal(testScrobbles, responseMessage, requestMessage);

            Assert.AreEqual(LastResponseStatus.Successful, scrobbleResponse.Status);
        }

        [Test]
        public async Task ScrobblesExistingCachedTracks()
        {
            var testScrobbles = GetTestScrobbles();

            // first request fails so something goes into the cache
            var responseMessage1 = TestHelper.CreateResponseMessage(HttpStatusCode.Forbidden, TrackApiResponses.TrackScrobbleBadAuthError);
            var scrobblesToCache = testScrobbles.Take(1);

            var scrobbleResponse1 = await ExecuteTestInternal(scrobblesToCache, responseMessage1);
            
            Assert.AreEqual(LastResponseStatus.Cached, scrobbleResponse1.Status);

            var scrobblesToSend = testScrobbles.Skip(1).Take(1);

            var requestMessage2 = GenerateExpectedRequestMessage(TrackApiResponses.TrackScrobbleTwoRequestBody);
            var responseMessage2 = TestHelper.CreateResponseMessage(HttpStatusCode.OK, TrackApiResponses.TrackScrobbleSuccess);
            var scrobbleResponse2 = await ExecuteTestInternal(scrobblesToSend, responseMessage2, requestMessage2);

            Assert.IsTrue(scrobbleResponse2.Success);
        }

        [Test]
        public async Task CorrectResponseWithBadAuth()
        {
            var requestMessage = GenerateExpectedRequestMessage(TrackApiResponses.TrackScrobbleMultipleRequestBody);

            var testScrobbles = GetTestScrobbles();
            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.Forbidden, TrackApiResponses.TrackScrobbleBadAuthError);
            var scrobbleResponse = await ExecuteTestInternal(testScrobbles, responseMessage, requestMessage);

            Assert.AreEqual(LastResponseStatus.Cached, scrobbleResponse.Status);

            // check actually cached
            var cached = await Scrobbler.GetCachedAsync();
            TestHelper.AssertSerialiseEqual(testScrobbles, cached);
        }

        [Test]
        public async Task CorrectResponseWhenRequestFailed()
        {
            var requestMessage = GenerateExpectedRequestMessage(TrackApiResponses.TrackScrobbleMultipleRequestBody);

            var testScrobbles = GetTestScrobbles();
            var responseMessage = TestHelper.CreateResponseMessage(HttpStatusCode.RequestTimeout, new byte[0]);
            var scrobbleResponse = await ExecuteTestInternal(testScrobbles, responseMessage, requestMessage);

            Assert.AreEqual(LastResponseStatus.Cached, scrobbleResponse.Status);

            // check actually cached
            var cached = await Scrobbler.GetCachedAsync();
            TestHelper.AssertSerialiseEqual(testScrobbles, cached);
        }
    }
}