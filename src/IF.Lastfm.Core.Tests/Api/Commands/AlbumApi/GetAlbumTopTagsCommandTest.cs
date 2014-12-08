using IF.Lastfm.Core.Api.Commands.AlbumApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Tests.Api.Commands.AlbumApi
{
    [TestClass]
    public class GetAlbumTopTagsCommandTest : CommandTestsBase
    {
        private GetAlbumTopTagsCommand _command;

        public GetAlbumTopTagsCommandTest()
        {
            _command = new GetAlbumTopTagsCommand(MAuth.Object)
            {
                AlbumName = "Believe",
                ArtistName = "Cher"
            };

            _command.SetParameters();
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.AreEqual(_command.Method, "album.getTopTags");
            Assert.AreEqual(_command.Parameters["album"], "Believe");
            Assert.AreEqual(_command.Parameters["artist"], "Cher");
        }

        [TestMethod]
        public async Task HandleSuccessResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTags));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);

        }

        [TestMethod]
        public async Task HandleEmptyResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [TestMethod]
        public async Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsError));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }
    }
}
