using IF.Lastfm.Core.Api;
using Moq;

namespace IF.Lastfm.Core.Tests
{
    public class MockLastFm
    {
        public Mock<IAuth> Auth { get; set; }

        public MockLastFm()
        {
            Auth = new Mock<IAuth>();
        }
    }
}