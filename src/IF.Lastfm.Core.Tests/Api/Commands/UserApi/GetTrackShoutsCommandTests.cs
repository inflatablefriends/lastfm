using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.TrackApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IF.Lastfm.Core.Tests.Api.Commands.UserApi
{
    [TestClass]
    public class GetUserApitestsCommandTests
    {
        private GetTrackShoutsCommand _command;

        private const string apiKey = "a6ab4b9376e54cdb06912bfbd9c1f288";
private const string apiSecret = "3aa7202fd1bc6d5a7ac733246cbccc4b";


        [TestMethod]
        public async void Constructor()
        {

            //var auth = new Core.Api.LastAuth(apiKey, apiSecret);
            //var userApi = new Core.Api.UserApi(auth);

            //var result = await userApi.GetTopAlbums()

        }
    }
}
