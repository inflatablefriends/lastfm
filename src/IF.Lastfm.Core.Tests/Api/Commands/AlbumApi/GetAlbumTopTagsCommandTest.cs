using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Album;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.AlbumApi
{
    public class GetAlbumTopTagsCommandTest : CommandTestsBase
    {
        private GetTopTagsCommand _command;

        public GetAlbumTopTagsCommandTest()
        {
            _command = new GetTopTagsCommand(MAuth.Object)
            {
                AlbumName = "Believe",
                ArtistName = "Cher"
            };

            _command.SetParameters();
        }

        [Test]
        public void Constructor()
        {
            Assert.AreEqual(_command.Method, "album.getTopTags");
            Assert.AreEqual(_command.Parameters["album"], "Believe");
            Assert.AreEqual(_command.Parameters["artist"], "Cher");
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTags));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsError));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }
    }
}
