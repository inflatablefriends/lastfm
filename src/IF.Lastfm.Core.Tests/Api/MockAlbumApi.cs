using IF.Lastfm.Core.Api;
using Moq;

namespace IF.Lastfm.Core.Tests.Api
{
    public class MockAlbumApi
    {
        public AlbumApi Object { get; private set; }

        public Mock<IAuth> Auth { get; private set; }

        public MockAlbumApi(Mock<IAuth> auth)
        {
            Auth = auth;
            
            Object = new AlbumApi(Auth.Object);
        }
    }
}