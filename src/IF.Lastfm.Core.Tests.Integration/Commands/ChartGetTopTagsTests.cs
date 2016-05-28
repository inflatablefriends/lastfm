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
            // TODO chart.getTopTracks ignores the limit parameter
            Assert.AreEqual(10, chartResponse.Content.Count);

            Assert.IsTrue(chartResponse.All(track => !string.IsNullOrEmpty(track.Name)));
        }

        [Test]
        public async Task GetTopArtists_Success()
        {
            var chartResponse = await Lastfm.Chart.GetTopArtistsAsync(PAGE, LIMIT);

            Assert.IsTrue(chartResponse.Success);
            Assert.AreEqual(PAGE, chartResponse.Page);
            Assert.AreEqual(LIMIT, chartResponse.PageSize);
            // TODO chart.getTopArtists ignores the limit parameter
            Assert.AreEqual(20, chartResponse.Content.Count);

            Assert.IsTrue(chartResponse.All(artist => !string.IsNullOrEmpty(artist.Name)));
        }

        [Test]
        public async Task GetTopTags_Success()
        {
            var chartResponse = await Lastfm.Chart.GetTopTagsAsync(PAGE, LIMIT);

            Assert.IsTrue(chartResponse.Success);
            // TODO chart.getTopTags ignores the page and limit parameters
            Assert.AreEqual(1, chartResponse.Page);
            Assert.AreEqual(50, chartResponse.TotalItems);
            Assert.AreEqual(50, chartResponse.Content.Count);

            Assert.IsTrue(chartResponse.All(tag => !string.IsNullOrEmpty(tag.Name)));
        }
    }
}