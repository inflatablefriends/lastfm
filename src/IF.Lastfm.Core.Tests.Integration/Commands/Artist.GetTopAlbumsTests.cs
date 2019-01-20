using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class ArtistGetTopAlbumsTests : CommandIntegrationTestsBase
    {
        private const string ARTIST_NAME = "The Knife";
        private const string ARTIST_MBID = "bf710b71-48e5-4e15-9bd6-96debb2e4e98";

        [Test]
        public async Task ArtistGetTopApbums_ByName_Success()
        {
            var response = await Lastfm.Artist.GetTopAlbumsAsync(ARTIST_NAME, false, 1, 10);
            var albums = response.Content;

            Assert.AreEqual(ARTIST_NAME, albums[0].ArtistName);
            Assert.AreEqual(10, albums.Count);
        }

        [Test]
        public async Task ArtistGetTopApbums_ByMbid_Success()
        {
            var response = await Lastfm.Artist.GetTopAlbumsByMbidAsync(ARTIST_MBID);
            var albums = response.Content;

            Assert.AreEqual(ARTIST_NAME, albums[0].ArtistName);
            Assert.AreEqual(20, albums.Count);
        }
    }
}
