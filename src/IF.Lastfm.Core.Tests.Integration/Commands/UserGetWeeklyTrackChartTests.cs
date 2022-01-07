namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    /*
    *  Sorry, but this is a bad test!
    *  It is based on expectation, that there is some activity in the Last.fm users profile and will fail when there were no scrobles for last three weeks.
    *  The tests should not expect any data input outside!

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

            //Values will vary from week to week so just checking that we got some values back
            Assert.IsNotEmpty(trackChart, "User.GetWeeklyChartListAsync - response.Content was empty");
            Assert.IsNotEmpty(trackChart.First().Name);
            Assert.IsNotEmpty(trackChart.First().ArtistName);

            //check that the two different weekly charts are not the same
            Assert.IsFalse(trackChart.First().Name == trackChartPrev.First().Name);
        }
    }
    */
}