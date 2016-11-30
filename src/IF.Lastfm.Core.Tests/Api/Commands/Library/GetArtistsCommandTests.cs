using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Library;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Library
{
    public class GetArtistsCommandTests : CommandTestsBase
    {
        [Test]
        public async Task HandleResponseMultiple()
        {
            var command = new GetArtistsCommand(MAuth.Object, "user");

            var expectedArtist = new LastArtist
            {
                Name = "Crystal Castles",
                PlayCount = 4219,
                Mbid = "b1570544-93ab-4b2b-8398-131735394202",
                Url = new Uri("https://www.last.fm/music/Crystal+Castles"),
                MainImage = new LastImageSet(
                    "https://lastfm-img2.akamaized.net/i/u/34s/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/64s/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/174s/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/300x300/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/f36a92bfbd7f8b579c91942c6a428d69.png")
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(LibraryApiResponses.LibraryGetArtistsMultiple));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedArtist, actual.Content[1]); // Testing the second track returned
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var command = new GetArtistsCommand(MAuth.Object, "user")
            {
                Page = 2,
                Count = 1
            };

            var expectedArtist = new LastArtist
            {
                Name = "Crystal Castles",
                PlayCount = 4219,
                Mbid = "b1570544-93ab-4b2b-8398-131735394202",
                Url = new Uri("https://www.last.fm/music/Crystal+Castles"),
                MainImage = new LastImageSet(
                    "https://lastfm-img2.akamaized.net/i/u/34s/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/64s/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/174s/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/300x300/f36a92bfbd7f8b579c91942c6a428d69.png",
                    "https://lastfm-img2.akamaized.net/i/u/f36a92bfbd7f8b579c91942c6a428d69.png")
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(LibraryApiResponses.LibraryGetArtistsSingle));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedArtist, actual.Content.Single()); // Testing the second track returned
        }

        [Test]
        public async Task HandleResponseError()
        {
            var command = new GetTracksCommand(MAuth.Object, "rj", "", "", DateTimeOffset.MinValue)
            {
                Count = 1
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(LibraryApiResponses.LibraryGetArtistsError));
            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}