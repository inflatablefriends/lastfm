using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class UserGetWeeklyChartTests : CommandIntegrationTestsBase
    {
        [Test]
        public async Task GetChartList_Success()
        {
            var response = await Lastfm.User.GetWeeklyChartListAsync(INTEGRATION_TEST_USER);
            var chartlist = response.Content;
            
            Assert.IsTrue(response.Success);
            Assert.IsNotEmpty(chartlist);
            Assert.AreEqual(typeof(double), chartlist.First().To.GetType());
            Assert.AreEqual(typeof(double), chartlist.First().From.GetType());
            Assert.AreEqual(typeof(DateTime), chartlist.First().ToDate.GetType());
            Assert.AreEqual(typeof(DateTime), chartlist.First().FromDate.GetType());
            Assert.That(chartlist.Last().ToDate, Is.EqualTo(DateTime.Now).Within(8).Days);
            Assert.That(chartlist.Last().FromDate, Is.EqualTo(DateTime.Now).Within(15).Days);
        }
    }
}
