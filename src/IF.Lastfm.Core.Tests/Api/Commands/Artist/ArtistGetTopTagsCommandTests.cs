using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Artist;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    
    public class ArtistGetTopTagsCommandTests : CommandTestsBase
    {
        private GetTopTagsCommand _command;

        [SetUp]
        public void Initialise()
        {
            _command = new GetTopTagsCommand(MAuth.Object);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var expectedTags = new List<LastTag>
            {
                new LastTag("electronic", "http://www.last.fm/tag/electronic", 100)
            };

            var file = GetFileContents("ArtistApi.ArtistGetTopTagsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsSingle));
            var parsed = await _command.HandleResponse(response);

            var expectedJson = expectedTags.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            parsed.AssertValues(true, 1, 1, 1, 1);
            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
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

            var file = GetFileContents("ArtistApi.ArtistGetTopTagsMultiple.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsMultiple));
            var parsed = await _command.HandleResponse(response);

            var expectedJson = expectedTags.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            parsed.AssertValues(true, 5, 5, 1, 1);
            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task HandleResponseEmpty()
        {
            var file = GetFileContents("ArtistApi.ArtistGetTopTagsEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsEmpty));
            var parsed = await _command.HandleResponse(response);

            parsed.AssertValues(true, 0, 0, 1, 1);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleResponseError()
        {
            var file = GetFileContents("ArtistApi.ArtistGetTopTagsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTopTagsError));
            var parsed = await _command.HandleResponse(response);

            parsed.AssertValues(false, 0, 0, 1, 1);
            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
            Assert.IsTrue(!parsed.Content.Any());
        }

    }
}