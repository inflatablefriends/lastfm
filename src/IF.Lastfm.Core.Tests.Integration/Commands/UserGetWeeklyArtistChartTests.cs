using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class UserGetWeeklyArtistChartTests : CommandIntegrationTestsBase
    {
        [Test]
        public async Task GetChartList_Success()
        {
            //call GetWeeklyChartList to get available weeks
            var weekList = await Lastfm.User.GetWeeklyChartListAsync(INTEGRATION_TEST_USER);
            var from = weekList.Content.Last().From;
            var to = weekList.Content.Last().To;
            //use the from and to params to call GetWeeklyArtistChart for the last week
            var response = await Lastfm.User.GetWeeklyArtistChartAsync(INTEGRATION_TEST_USER, from, to);
            var artistChart = response.Content;
            
            Assert.IsTrue(response.Success);

            // Test account hasn't been scrobling so the weekly charts haven't been generating
            // Charts are currently empty as expected

            //Values will vary from week to week so just checking that we got some values back
            //Assert.IsNotEmpty(artistChart);
            //Assert.IsNotEmpty(artistChart.First().Name);
        }
    }
}