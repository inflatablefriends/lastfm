using System;
using System.Collections.Generic;
using IF.Lastfm.Core.Api.Commands.Library;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Library
{
    public class RemoveScrobbleCommandTests : CommandTestsBase
    {
        private readonly RemoveScrobbleCommand _command;

        private const string ARTIST = "Hot Chip";
        private const string TRACK = "Over and Over";
        private static DateTimeOffset _timePlayed = new DateTimeOffset(2015, 04, 10, 18, 0, 0, TimeSpan.Zero);

        public RemoveScrobbleCommandTests()
        {
            _command = new RemoveScrobbleCommand(MAuth.Object, "Hot Chip", "Over and Over", _timePlayed);
        }

        [Test]  
        public void ParametersCorrect()
        {
            var expected = new Dictionary<string, string>
            {
                {"artist", ARTIST},
                {"track", TRACK},
                {"timestamp", "1428688800"}
            };

            _command.SetParameters();
            _command.Parameters.Remove("disablecachetoken");

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }
    }
}