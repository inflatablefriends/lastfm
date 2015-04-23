using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Library;

namespace IF.Lastfm.Core.Tests.Api.Commands.Library
{
    public class LibraryGetTracksCommandTests : CommandTestsBase
    {
        private readonly GetTracksCommand _command;

        public LibraryGetTracksCommandTests()
        {
            _command = new GetTracksCommand(MAuth.Object, "rj", "", "", DateTime.MinValue)
            {
                Count = 1
            };            
        }
        
        [Test]
        public async Task HandleResponseMultiple()
        {
            //Testing the second track returned
            var expectedTrack = new LastTrack
            {
                ArtistName = "Stevie Wonder",
                Duration = new TimeSpan(0, 4, 8),
                PlayCount = 56,
                Mbid = "0161855d-0b98-4f2d-b2ab-446dbd8d6759",
                Name = "Superstition",
                ArtistMbid = "1ee18fb3-18a6-4c7f-8ba0-bc41cdd0462e",
                AlbumName = "Number Ones",
                Url = new Uri("http://www.last.fm/music/Stevie+Wonder/_/Superstition", UriKind.Absolute),
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/99695819.jpg",
                    "http://userserve-ak.last.fm/serve/64s/99695819.jpg",
                    "http://userserve-ak.last.fm/serve/126/99695819.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/99695819.jpg")

            };

            await CompareResultsMultiple(_command, expectedTrack, LibraryApiResponses.LibraryGetTracksMultiple, 1);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var expectedTrack = new LastTrack
            {
                ArtistName = "Dire Straits",
                Duration = new TimeSpan(0, 5, 47),
                PlayCount = 81,
                Mbid = "0317e524-7f70-4910-bc12-95dd468a29fc",
                Name = "Sultans of Swing",
                ArtistMbid = "614e3804-7d34-41ba-857f-811bad7c2b7a",
                AlbumName = "Dire Straits (Remastered)",
                Url = new Uri("http://www.last.fm/music/Dire+Straits/_/Sultans+of+Swing", UriKind.Absolute),
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/56827829.jpg",
                    "http://userserve-ak.last.fm/serve/64s/56827829.jpg",
                    "http://userserve-ak.last.fm/serve/126/56827829.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/56827829.jpg")
            };

            var expected = new List<LastTrack> { expectedTrack };

            await CompareResultsSingle(_command, expected, LibraryApiResponses.LibraryGetTracksSingle);
        }
        
        [Test]
        public async Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetInfoMissing));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
