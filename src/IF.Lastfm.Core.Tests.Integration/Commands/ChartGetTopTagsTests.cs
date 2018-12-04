using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    /// <summary>
    /// TODO Last.fm doesn't return results consistently in this API
    /// If these tests break that might mean the API has started to work as expected wrt page and limit parameters
    /// </summary>
    [TestFixture]
    public class ChartApiTests : CommandIntegrationTestsBase
    {
        private const int PAGE = 2;
        public const int LIMIT = 30;

        [Test]
        public async Task GetTopTracks_Success()
        {
            var chartResponse = await Lastfm.Chart.GetTopTracksAsync(PAGE, LIMIT);
            
            Assert.IsTrue(chartResponse.Success);
            Assert.AreEqual(PAGE, chartResponse.Page);
            Assert.AreEqual(LIMIT, chartResponse.PageSize);
            Assert.AreEqual(LIMIT, chartResponse.Content.Count);

            Assert.IsTrue(chartResponse.All(track => !string.IsNullOrEmpty(track.Name)));
        }

        [Test]
        public async Task GetTopArtists_Success()
        {
            //2018-12-04: there seems to be a bug in the lastfm api right now
            //it is returning LIMIT*2 items if PAGE is anything but 1..
            int PAGE1 = 1;
            var chartResponse = await Lastfm.Chart.GetTopArtistsAsync(PAGE1, LIMIT);

            Assert.IsTrue(chartResponse.Success);
            Assert.AreEqual(PAGE1, chartResponse.Page, "Error in Page");
            Assert.AreEqual(LIMIT, chartResponse.PageSize, "Error in PageSize");
            Assert.AreEqual(LIMIT, chartResponse.Content.Count, "Error in ContentCount");

            Assert.IsTrue(chartResponse.All(artist => !string.IsNullOrEmpty(artist.Name)));
        }

        [Test]
        public async Task GetTopTags_Success()
        {
            var chartResponse = await Lastfm.Chart.GetTopTagsAsync(PAGE, LIMIT);

            Assert.IsTrue(chartResponse.Success);
            Assert.AreEqual(LIMIT, chartResponse.TotalItems);
            Assert.AreEqual(LIMIT, chartResponse.Content.Count);

            Assert.AreNotEqual(PAGE, chartResponse.Page); // TODO chart.getTopTags ignores the page parameter

            Assert.IsTrue(chartResponse.All(tag => !string.IsNullOrEmpty(tag.Name)));
        }
    }
}