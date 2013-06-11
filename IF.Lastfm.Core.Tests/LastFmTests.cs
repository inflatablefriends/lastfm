using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IF.Lastfm.Core.Tests
{
    [TestClass]
    public class LastFmTests
    {
        [TestMethod]
        public void ApiUrlFormatReturnsCorrectly()
        {
            const string expected = "https://ws.audioscrobbler.com/2.0/?method=tobias.funke&api_key=suddenvalley&blue=performance&format=json&uncle=t-bag";

            var actual = LastFm.FormatApiUrl("tobias.funke", "suddenvalley", new Dictionary<string, string>
                {
                    {"uncle", "t-bag"},
                    {"blue", "performance"}
                }, true);

            Assert.AreEqual(expected, actual);
        }
    }
}
