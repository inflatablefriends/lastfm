using IF.Lastfm.Core.Api.Commands.TrackApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Tests.Api.Commands.TrackApi
{
    [TestClass]
    public class GetTrackShoutsCommandTests : CommandTestsBase
    {
        private GetTrackShoutsCommand _command;

        public GetTrackShoutsCommandTests()
        {
            _command = new GetTrackShoutsCommand(MAuth.Object, "Genesis", "Grimes")
                       {
                           Autocorrect = true,
                           Page = 5,
                           Count = 7
                       };
        }

        [TestMethod]
        public override void Constructor()
        {
            Assert.AreEqual(_command.TrackName, "Genesis");
            Assert.AreEqual(_command.ArtistName, "Grimes");
            Assert.AreEqual(_command.Method, "track.getShouts");
        }

        [TestMethod]
        public async override Task HandleSuccessResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShouts));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(parsed.Page == 5);
            Assert.IsTrue(parsed.Content.Count() == 7);
        }

        [TestMethod]
        public async override Task HandleResponseSingle()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsSingle));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(parsed.Content.Count() == 1);
        }

        [TestMethod]
        public async override Task HandleEmptyResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [TestMethod]
        public async override Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsError));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }
    }
}
