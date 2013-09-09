using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.TrackApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TestData.TrackGetShouts));

            var parsed = await _command.HandleResponse(response);
            
            Assert.IsTrue(parsed.Page == 5);
            Assert.IsTrue(parsed.Content.Count() == 7);
        }

        /// <summary>
        /// The shouts API uses a different schema when there's only one shout in the page
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task HandleResponseSingle()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TestData.TrackGetShoutsSingle));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Content.Count() == 1);
        }
    }
}
