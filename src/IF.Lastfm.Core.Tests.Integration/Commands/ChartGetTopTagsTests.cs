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
            var chartResponse = await Lastfm.Chart.GetTopArtistsAsync(PAGE, LIMIT);

            Assert.IsTrue(chartResponse.Success);
            Assert.AreEqual(PAGE, chartResponse.Page, "Error in Page");
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