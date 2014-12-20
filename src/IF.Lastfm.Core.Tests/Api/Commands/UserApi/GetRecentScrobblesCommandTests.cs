using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.UserApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IF.Lastfm.Core.Api.Commands.LibraryApi;
using IF.Lastfm.Core.Objects;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IF.Lastfm.Core.Tests.Api.Commands.UserApi
{
    [TestClass]
    public class GetRecentScrobblesCommandTests : CommandTestsBase
    {
        private GetRecentScrobblesCommand _command;

        //private const string apiKey = "a6ab4b9376e54cdb06912bfbd9c1f288";
        //private const string apiSecret = "3aa7202fd1bc6d5a7ac733246cbccc4b";

        public GetRecentScrobblesCommandTests()
        {
            _command = new GetRecentScrobblesCommand(MAuth.Object, "rj", System.DateTime.MinValue)
            {
                Count = 1
            };
        }


        [TestMethod]
        public async Task HandleSuccessResponse_User_GetRecentScrobbles_Multiple()
        {
            //Testing the second track returned
            var expectedTrack = new LastTrack
            {
                ArtistName = "The Who",
                // Duration = new TimeSpan(0, 3, 47),
                //TotalPlayCount = 81,
                TimePlayed = new DateTime(2014, 12, 19, 16, 13, 55,DateTimeKind.Utc),
                Mbid = "79f3dc97-2297-47ee-8556-9a1bb4b48d53",
                Name = "Pinball Wizard",
                ArtistMbid = "9fdaa16b-a6c4-4831-b87c-bc9ca8ce7eaa",
                AlbumName = "Tommy (Remastered)",
                Url = new Uri("http://www.last.fm/music/The+Who/_/Pinball+Wizard", UriKind.Absolute),

                // Id = "1934",
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/64s/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/126/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/35234991.jpg")

            };


            await CheckResult_MultipleSample(_command, expectedTrack, 2, UserApiResponses.UserGetRecentTracksMultiple);
        }

        [TestMethod]
        public async Task HandleSuccessResponse_User_GetRecentScrobbles_Single()
        {
            //Broken until single objects work in the parser
            var expectedTrack = new LastTrack
            {
                ArtistName = "Rick James",
               // Duration = new TimeSpan(0, 3, 47),
                //TotalPlayCount = 81,
                Mbid = "",
                Name = "Super Freak (Part 1) - 1981 Single Version",
                ArtistMbid = "cba9cec2-be8d-41bd-91b4-a1cd7de39b0c",

                TimePlayed = new DateTime(2014,12,20,10,16,52, DateTimeKind.Utc),
                AlbumName = "The Definitive Collection",
                Url = new Uri("http://www.last.fm/music/Rick+James/_/Super+Freak+(Part+1)+-+1981+Single+Version", UriKind.Absolute),

               // Id = "1934",
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/64s/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/126/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/90462319.jpg")

            };


            var expected = new List<LastTrack> { expectedTrack };

            await CheckResult_Single(_command, expected, UserApiResponses.UserGetRecentTracksSingle);

        }

        [TestMethod]
        public async Task HandleErrorResponse_User_GetRecentScrobbles()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksError));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
        }

    }
}
