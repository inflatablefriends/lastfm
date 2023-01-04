using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class UserGetWeeklyTrackChartTests : CommandIntegrationTestsBase
    {

        [Test]
        public async Task GetTrackChart_Success()
        {
            //call GetWeeklyChartList to get available weeks
            var weekList = await Lastfm.User.GetWeeklyChartListAsync(INTEGRATION_TEST_USER);
            var fromLastWeek = weekList.Content.Last().From;
            var toLastWeek = weekList.Content.Last().To;
            var fromPrevWeek = weekList.Content[weekList.Content.Count - 2].From;
            var toPrevWeek = weekList.Content[weekList.Content.Count - 2].To;

            //use the from and to params to call GetWeeklyArtistChart for the last week
            var response = await Lastfm.User.GetWeeklyTrackChartAsync(INTEGRATION_TEST_USER, fromLastWeek, toLastWeek);
            var trackChart = response.Content;
            //get weekly chart for the week before
            var responsePrev = await Lastfm.User.GetWeeklyTrackChartAsync(INTEGRATION_TEST_USER, fromPrevWeek, toPrevWeek);
            var trackChartPrev = responsePrev.Content;
            
            Assert.IsTrue(response.Success, "User.GetWeeklyChartListAsync - response.Success was not true");

            // Test account hasn't been scrobling so the weekly charts haven't been generating
            // Charts are currently empty as expected

            //Values will vary from week to week so just checking that we got some values back
            //Assert.IsNotEmpty(trackChart, "User.GetWeeklyChartListAsync - response.Content was empty");
            //Assert.IsNotEmpty(trackChart.First().Name);
            //Assert.IsNotEmpty(trackChart.First().ArtistName);

            ////check that the two different weekly charts are not the same
            //Assert.IsFalse(trackChart.First().Name == trackChartPrev.First().Name);
        }
    }
}