using IF.Lastfm.Core.Api.Commands.ArtistApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    [TestClass]
    public class ArtistGetTopTagsCommandTests : CommandTestsBase
    {
        private ArtistGetTopTagsCommand _command;

        [TestInitialize]
        public void Initialise()
        {
            _command = new ArtistGetTopTagsCommand(MAuth.Object, "");
        }

        [TestMethod]
        public async Task HandleResponseSingle()
        {
            var expectedTags = new List<LastTag>
            {
                new LastTag("electronic", "http://www.last.fm/tag/electronic", 100)
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsSingle));
            var parsed = await _command.HandleResponse(response);

            var expectedJson = expectedTags.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            parsed.AssertValues(true, 1, 1, 1, 1);
            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [TestMethod]
        public async Task HandleResponseMultiple()
        {
            var expectedTags = new List<LastTag>
            {
                new LastTag("electronic", "http://www.last.fm/tag/electronic", 100),
                new LastTag("experimental", "http://www.last.fm/tag/experimental", 100),
                new LastTag("Bandcamp", "http://www.last.fm/tag/bandcamp", 100),
                new LastTag("ambient", "http://www.last.fm/tag/ambient", 50),
                new LastTag("psychedelic", "http://www.last.fm/tag/psychedelic", 50)
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsMultiple));
            var parsed = await _command.HandleResponse(response);

            var expectedJson = expectedTags.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            parsed.AssertValues(true, 5, 5, 1, 1);
            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [TestMethod]
        public async Task HandleResponseEmpty()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsEmpty));
            var parsed = await _command.HandleResponse(response);

            parsed.AssertValues(true, 0, 0, 1, 1);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [TestMethod]
        public async Task HandleResponseError()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsError));
            var parsed = await _command.HandleResponse(response);

            parsed.AssertValues(false, 0, 0, 1, 1);
            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
            Assert.IsTrue(!parsed.Content.Any());
        }

    }
}