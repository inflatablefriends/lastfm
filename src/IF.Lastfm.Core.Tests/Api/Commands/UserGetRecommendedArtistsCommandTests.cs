using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.UserApi;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    [TestClass]
    public class UserGetRecommendedArtistsCommandTests : CommandTestsBase
    {
        private GetRecommendedArtistsCommand _commmand;

        [TestInitialize]
        public void Initialise()
        {
            _commmand = new GetRecommendedArtistsCommand(MAuth.Object);
        }

        [TestMethod]
        public async Task HandleResponseSingle()
        {
            var expectedArtist = new LastArtist
            {
                Name = "Liars",
                Mbid = "03098741-08b3-4dd7-b3f6-1b0bfa2c879c",
                Url = new Uri("http://www.last.fm/music/Liars"),
                MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/2261874.jpg",
                    "http://userserve-ak.last.fm/serve/64/2261874.jpg",
                    "http://userserve-ak.last.fm/serve/126/2261874.jpg",
                    "http://userserve-ak.last.fm/serve/252/2261874.jpg",
                    "http://userserve-ak.last.fm/serve/_/2261874/Liars.jpg")
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecommendedArtistsSingle));
            var parsed = await _commmand.HandleResponse(response);
            
            Assert.IsTrue(parsed.Success);

            var expectedJson = expectedArtist.WrapEnumerable().TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [TestMethod]
        public async Task HandleResponseMultiple()
        {
            var expectedArtists = new List<LastArtist>
            {
                new LastArtist
                {
                    Name = "Liars",
                    Mbid = "03098741-08b3-4dd7-b3f6-1b0bfa2c879c",
                    Url = new Uri("http://www.last.fm/music/Liars"),
                    MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/2261874.jpg",
                        "http://userserve-ak.last.fm/serve/64/2261874.jpg",
                        "http://userserve-ak.last.fm/serve/126/2261874.jpg",
                        "http://userserve-ak.last.fm/serve/252/2261874.jpg",
                        "http://userserve-ak.last.fm/serve/_/2261874/Liars.jpg")
                },
                new LastArtist
                {
                    Name = "The Haxan Cloak",
                    Mbid = "c9224968-d1b7-455f-84f4-2ceefa7d3a4e",
                    Url = new Uri("http://www.last.fm/music/The+Haxan+Cloak"),
                    MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/92960643.png",
                        "http://userserve-ak.last.fm/serve/64/92960643.png",
                        "http://userserve-ak.last.fm/serve/126/92960643.png",
                        "http://userserve-ak.last.fm/serve/252/92960643.png",
                        "http://userserve-ak.last.fm/serve/500/92960643/The+Haxan+Cloak+THC_PNG_080913_04.png")
                },
                new LastArtist
                {
                    Name = "Cloetta Paris",
                    Mbid = "24a9af30-cb7a-4456-ba3d-6daba1245b26",
                    Url = new Uri("http://www.last.fm/music/Cloetta+Paris"),
                    MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/10586607.jpg",
                        "http://userserve-ak.last.fm/serve/64/10586607.jpg",
                        "http://userserve-ak.last.fm/serve/126/10586607.jpg",
                        "http://userserve-ak.last.fm/serve/252/10586607.jpg",
                        "http://userserve-ak.last.fm/serve/500/10586607/Cloetta+Paris+CloettaParis.jpg")
                },
            };


            var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecommendedArtistsMultiple));
            var parsed = await _commmand.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var expectedJson = expectedArtists.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }
    }
}