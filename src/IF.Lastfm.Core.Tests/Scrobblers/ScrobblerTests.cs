using IF.Lastfm.Core.Scrobblers;
using System.Net.Http;

namespace IF.Lastfm.Core.Tests.Scrobblers
{
    public class ScrobblerTests : ScrobblerTestsBase
    {
        protected override ScrobblerBase GetScrobbler()
        {
            var httpClient = new HttpClient(FakeResponseHandler);
            return new MemoryScrobbler(MockAuth.Object, httpClient);
        }
    }
}
