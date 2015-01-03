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

            const string expectedHash = "41919D4DF853702763556BFE2085406B";
            var actual = _auth.GenerateMethodSignature(method, parameters);

            Assert.AreEqual(expectedHash, actual);
        }
    }
}