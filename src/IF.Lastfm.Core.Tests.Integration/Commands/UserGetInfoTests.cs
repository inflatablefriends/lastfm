using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class UserGetInfoTests : CommandIntegrationTestsBase
    {
        [Test]
        public async Task GetInfo_Success()
        {
            var response = await Lastfm.User.GetInfoAsync(INTEGRATION_TEST_USER);
            var user = response.Content;
            
            Assert.AreEqual(INTEGRATION_TEST_USER, user.Name);
            Assert.IsTrue(user.Playcount > 0);
        }
    }
}
