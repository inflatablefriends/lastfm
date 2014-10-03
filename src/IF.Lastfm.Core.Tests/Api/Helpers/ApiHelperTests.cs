using IF.Lastfm.Core.Api.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IF.Lastfm.Core.Tests.Api.Helpers
{
    internal enum TestApiEnum
    {
        Unknown = 0,

        [ApiName("dogs")]
        Dogs,

        [ApiName("cats")]
        Cats
    }

    [TestClass]
    public class ApiHelperTests
    {
        [TestMethod]
        public void GetApiNameReturnsAttribute()
        {
            var enumValue = TestApiEnum.Dogs;

            var expected = "dogs";
            var actual = enumValue.GetApiName();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetApiNameReturnsValueIfNoAttribute()
        {
            var enumValue = TestApiEnum.Unknown;

            var expected = "Unknown";
            var actual = enumValue.GetApiName();

            Assert.AreEqual(expected, actual);
        }
    }
}
