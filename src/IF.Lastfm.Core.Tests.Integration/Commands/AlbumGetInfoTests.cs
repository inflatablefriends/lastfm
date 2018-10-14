using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Integration.Commands
{
    public class AlbumGetInfoTests : CommandIntegrationTestsBase
    {
        private const string ARTIST_NAME = "The Knife";
        private const string ALBUM_NAME = "Deep Cuts";

        [Test]
        public async Task GetInfo_ForUser_Success()
        {
            var response = await Lastfm.Album.GetInfoAsync(ARTIST_NAME, ALBUM_NAME, false, INTEGRATION_TEST_USER);
            var album = response.Content;

            Assert.AreEqual(ARTIST_NAME, album.ArtistName);
            Assert.Greater(album.UserPlayCount, 0);
        }
    }
}
