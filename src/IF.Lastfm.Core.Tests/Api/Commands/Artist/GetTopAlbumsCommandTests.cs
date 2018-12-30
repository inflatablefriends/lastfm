using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Artist;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Artist
{
    public class GetTopAlbumsCommandTests : CommandTestsBase
    {
        private GetTopAlbumsCommand _commandArtist;
        private GetTopAlbumsCommand _commandMbid;

        [SetUp]
        public void Initialise()
        {
            _commandArtist = new GetTopAlbumsCommand(MAuth.Object)
            {
                ArtistName = "Steely Dan"
            };
            _commandMbid = new GetTopAlbumsCommand(MAuth.Object)
            {
                ArtistMbid = "e01c3376-15fa-40d7-b747-5f219bdefdd7"
            };
        }

        [Test]
        public async Task GetTopAlbums_HandleResponse_Success()
        {   
            _commandArtist.SetParameters();
            string artistValue;
            Assert.IsTrue(_commandArtist.Parameters.TryGetValue("artist", out artistValue));
            Assert.AreEqual("Steely Dan", artistValue);

            var file = GetFileContents("ArtistApi.ArtistGetTopAlbumsSuccess.json");
            var response = CreateResponseMessage(file);
            var parsed = await _commandArtist.HandleResponse(response);
            
            Assert.IsTrue(parsed.Success, "parsed.success should be true");
            Assert.AreEqual(10, parsed.Content.Count);
            Assert.AreEqual(10, parsed.PageSize);
            Assert.AreEqual(16897, parsed.TotalItems);
            Assert.AreEqual(1690, parsed.TotalPages);
            Assert.AreEqual("Steely Dan", parsed.Content[0].ArtistName);
        }

        [Test]
        public void GetTopAlbums_ByMbid_Success()
        {
            _commandMbid.SetParameters();
            string mbidValue;
            Assert.IsTrue(_commandMbid.Parameters.TryGetValue("mbid", out mbidValue));
            Assert.AreEqual("e01c3376-15fa-40d7-b747-5f219bdefdd7", mbidValue);
        }
        
        [Test]
        public async Task GetTopAlbums_HandleResponse_Missing()
        {
            var file = GetFileContents("ArtistApi.ArtistGetTopAlbumsMissing.json");
            var response = CreateResponseMessage(file);
            
            var parsed = await _commandArtist.HandleResponse(response);

            Assert.IsFalse(parsed.Success, "parsed.success should be false");
            Assert.AreEqual(LastResponseStatus.MissingParameters, parsed.Status);
        }
    }
}
