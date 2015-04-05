using IF.Lastfm.Core.Scrobblers;
using System.Net.Http;
using Scrobbler = IF.Lastfm.Core.Scrobblers.Scrobbler;

namespace IF.Lastfm.Core.Tests.Scrobblers
{
    public class ScrobblerTests : ScrobblerTestsBase
    {
        protected override IScrobbler GetScrobbler()
        {
            var httpClient = new HttpClient(FakeResponseHandler);
            return new Scrobbler(MockAuth.Object, httpClient);
        }
    }
}
