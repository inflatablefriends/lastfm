using IF.Lastfm.Core.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace IF.Lastfm.Core.Tests.Api
{
    [TestClass]
    public class LastAuthTests
    {
        private ILastAuth _auth;

        [TestInitialize]
        public void Initialise()
        {
            _auth = new LastAuth("user", "pass");
        }

        [TestMethod]
        public void GeneratesCorrectMethodSignature()
        {
            const string method = "test.method";
            var parameters = new Dictionary<string, string>
            {
                {"test", "value"},
                {"test2", "value2"}
            };

            const string expectedHash = "910231882F8AA6D4BBACB7C5FE5F5CC8";
            var actual = _auth.GenerateMethodSignature(method, parameters);

            Assert.AreEqual(expectedHash, actual);
        }
    }
}