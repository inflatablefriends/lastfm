using System.Collections.Generic;
using IF.Lastfm.Core.Api.Commands.Library;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Library
{
    public class RemoveTrackCommandTests : CommandTestsBase
    {
        private readonly RemoveTrackCommand _command;

        private const string ARTIST = "Hot Chip";
        private const string TRACK = "Over and Over";

        public RemoveTrackCommandTests()
        {
            _command = new RemoveTrackCommand(MAuth.Object, "Hot Chip", "Over and Over");
        }

        [Test]
        public void ParametersCorrect()
        {
            var expected = new Dictionary<string, string>
            {
                {"artist", ARTIST},
                {"track", TRACK}
            };

            _command.SetParameters();
            _command.Parameters.Remove("disablecachetoken");

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }
    }
}