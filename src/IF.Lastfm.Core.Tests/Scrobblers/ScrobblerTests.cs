using IF.Lastfm.Core.Scrobblers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using IF.Lastfm.Core.Helpers;
using Scrobbler = IF.Lastfm.Core.Scrobblers.Scrobbler;

namespace IF.Lastfm.Core.Tests.Scrobblers
{
    [TestClass]
    public class ScrobblerTests : ScrobblerTestsBase
    {
        protected override IScrobbler GetScrobbler()
        {
            var httpClient = new HttpClient(FakeResponseHandler);
            return new Scrobbler(MockAuth.Object, httpClient);
        }
    }
}
