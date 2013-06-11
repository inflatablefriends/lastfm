using IF.Lastfm.Core.Api;
using Moq;

namespace IF.Lastfm.Core.Tests
{
    public class MockLastFm
    {
        public LastFm Object { get; set; }

        public Mock<IAuth> Auth { get; set; }

        public MockLastFm()
        {
            Auth = new Mock<IAuth>();

            Object = new LastFm
                {
                    Auth = Auth.Object
                };
        }
    }
}