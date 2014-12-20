using System;
using System.Collections.Generic;
using System.Linq;
using IF.Lastfm.Core.Api.Commands.LibraryApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IF.Lastfm.Core.Tests.Api.Commands.LibraryApi
{
    [TestClass]
    public class GetTracksCommandTests : CommandTestsBase
    {
        private GetTracksCommand _command;

        public GetTracksCommandTests()
        {


            _command = new GetTracksCommand(MAuth.Object, "rj", "", "", DateTime.MinValue)
            {
                Count = 1
            };
            //_command = new GetTracksCommand(MAuth.Object, )
            //{
            //    AlbumName = "Ray of Light",
            //    ArtistName = "Madonna"
            //};
        //_command.SetParameters();

            
        }


        [TestMethod]
        public async Task HandleSuccessResponseSingleTrack()
        {
           
                
            var expectedTrack = new LastTrack
            {
                ArtistName = "Dire Straits",
                Duration = new TimeSpan(0, 3, 47),
                TotalPlayCount = 81,
                Mbid = "0317e524-7f70-4910-bc12-95dd468a29fc",
                Name = "Sultans of Swing",
                ArtistMbid = "614e3804-7d34-41ba-857f-811bad7c2b7a",
                Url = new Uri("http://www.last.fm/music/Dire+Straits/_/Sultans+of+Swing", UriKind.Absolute),

                Id = "1934",
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/56827829.jpg",
                    "http://userserve-ak.last.fm/serve/64s/56827829.jpg",
                    "http://userserve-ak.last.fm/serve/126/56827829.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/56827829.jpg")
                    
            };

            var response = CreateResponseMessage(Encoding.UTF8.GetString(LibraryApiResponses.LibraryGetTracksSingle));
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var actual = parsed.Content;
            Assert.IsTrue(actual.Count() == 1);
            actual = null;

            var expectedJson = JsonConvert.SerializeObject(expectedTrack, Formatting.Indented);
            var actualJson = JsonConvert.SerializeObject(parsed.Content, Formatting.Indented);

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [TestMethod]
        public async Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetInfoMissing));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
        }
    }
}
