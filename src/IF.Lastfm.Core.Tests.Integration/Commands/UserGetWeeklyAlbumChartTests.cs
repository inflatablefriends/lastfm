using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class UserGetWeeklyAlbumChartTests : CommandIntegrationTestsBase
    {
        [Test]
        public async Task GetAlbumChart_Success()
        {
            //call GetWeeklyChartList to get available weeks
            var weekList = await Lastfm.User.GetWeeklyChartListAsync(INTEGRATION_TEST_USER);
            var from = weekList.Content.Last().From;
            //var to = weekList.Content.Last().To;
            //use the from and to params to call GetWeeklyArtistChart for the last week
            var response = await Lastfm.User.GetWeeklyAlbumChartAsync(INTEGRATION_TEST_USER, from);
            var artistChart = response.Content;

            Assert.IsTrue(response.Success);

            //Values will vary from week to week so just checking that we got some values back
            Assert.IsNotEmpty(artistChart);
            Assert.IsNotEmpty(artistChart.First().Name);
            Assert.IsNotEmpty(artistChart.First().ArtistName);
        }
    }
}