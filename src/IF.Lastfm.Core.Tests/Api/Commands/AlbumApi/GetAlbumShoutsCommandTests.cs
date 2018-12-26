using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Album;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.AlbumApi
{
    public class GetAlbumShoutsCommandTests : CommandTestsBase
    {
        private GetShoutsCommand _command;

        public GetAlbumShoutsCommandTests()
        {
            _command = new GetShoutsCommand(MAuth.Object, "Visions", "Grimes");
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetShoutsMultiple.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetShoutsMultiple));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);

            var expectedShouts = new List<LastShout>
            {
                new LastShout("uhIgor", "Nunca vou cansar disso.", "Thu, 23 Oct 2014 02:20:30"),
                new LastShout("Zachary-K",
                    "Oblivion is the best song on whole album. Maybe i dig it but i don't want to really. Or i'm not in the right mood for it.",
                    "Wed, 17 Sep 2014 21:04:36")
            };
            
            var expectedJson = JsonConvert.SerializeObject(expectedShouts, Formatting.Indented);
            var actualJson = JsonConvert.SerializeObject(parsed.Content, Formatting.Indented);

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var file = GetFileContents("AlbumApi.AlbumGetShoutsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetShoutsSingle));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(parsed.Content.Count() == 1);
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetShoutsEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetShoutsEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetShoutsAlbumMissing.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetShoutsAlbumMissing));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }
    }
}