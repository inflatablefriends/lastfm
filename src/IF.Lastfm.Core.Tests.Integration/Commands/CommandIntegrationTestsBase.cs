using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Tests.Api.Commands;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class CommandIntegrationTestsBase : CommandTestsBase
    {
        public const string INTEGRATION_TEST_USER = "inflatabledemo";
        public const string INTEGRATION_TEST_PASSWORD = "inflatabledemo#";

        protected LastAuth Auth { get; private set; }

        [SetUp]
        public void Initialise()
        {
            Auth = new LastAuth(LastFm.TEST_APIKEY, LastFm.TEST_APISECRET);

            Auth.GetSessionTokenAsync(INTEGRATION_TEST_USER, INTEGRATION_TEST_PASSWORD).Wait();
        }
    }
}