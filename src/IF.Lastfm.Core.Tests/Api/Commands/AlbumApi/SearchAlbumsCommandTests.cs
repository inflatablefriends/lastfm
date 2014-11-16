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
    public class SearchAlbumsCommandTests : CommandTestsBase
    {
        private SearchAlbumsCommand _command;

        public SearchAlbumsCommandTests()
        {
            _command = new SearchAlbumsCommand(MAuth.Object, "By the throat")
                       {
                           Page = 2,
                           Count = 3
                       };

            _command.SetParameters();
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.AreEqual(_command.Method, "album.search");

            Assert.AreEqual(_command.Parameters["album"], "By the throat");
            Assert.AreEqual(_command.Parameters["page"], "2");
            Assert.AreEqual(_command.Parameters["limit"], "3");
        }

        [TestMethod]
        public async Task HandleSuccessResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearch));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(parsed.Page == 2);
            Assert.IsTrue(parsed.Content.Count() == 3);
        }

        [TestMethod]
        public async Task HandleResponseSingle()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchSingle));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(parsed.Content.Count() == 1);
        }

        [TestMethod]
        public async Task HandleEmptyResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [TestMethod]
        public async Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchError));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }
    }
}
