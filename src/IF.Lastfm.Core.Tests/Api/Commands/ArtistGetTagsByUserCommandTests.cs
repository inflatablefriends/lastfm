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
    public class ArtistGetTagsByUserCommandTests : CommandTestsBase
    {
        private ArtistGetTagsByUserCommand _command;

        [TestInitialize]
        public void Initialise()
        {
            _command = new ArtistGetTagsByUserCommand(MAuth.Object, "", "");
        }

        [TestMethod]
        public async Task HandleResponseSingle()
        {
            var expectedTags = new List<LastTag>
            {
                new LastTag("the fate of the world is safe in crystal castles", "http://www.last.fm/tag/the%20fate%20of%20the%20world%20is%20safe%20in%20crystal%20castles")
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTagsSingle));
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
                new LastTag("the fate of the world is safe in crystal castles", "http://www.last.fm/tag/the%20fate%20of%20the%20world%20is%20safe%20in%20crystal%20castles"),
                new LastTag("if this were a pokemon i would catch it", "http://www.last.fm/tag/if%20this%20were%20a%20pokemon%20i%20would%20catch%20it")
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTagsMultiple));
            var parsed = await _command.HandleResponse(response);

            var expectedJson = expectedTags.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            parsed.AssertValues(true, 2, 2, 1, 1);
            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [TestMethod]
        public async Task HandleResponseEmpty()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTagsEmpty));
            var parsed = await _command.HandleResponse(response);

            parsed.AssertValues(true, 0, 0, 1, 1);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [TestMethod]
        public async Task HandleResponseError()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTagsError));
            var parsed = await _command.HandleResponse(response);

            parsed.AssertValues(false, 0, 0, 1, 1);
            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
            Assert.IsTrue(!parsed.Content.Any());
        }
    }
}