using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    [TestClass]
    public class UserGetRecentTracksCommandTests : CommandTestsBase
    {
        private GetRecentTracksCommand _command;

        [TestInitialize]
        public void Initialise()
        {
            _command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };
        }

        [TestMethod]
        public async Task HandleResponseMultiple()
        {
            var expectedTrack = new LastTrack
            {
                ArtistName = "The Who",
                TimePlayed = new DateTime(2014, 12, 19, 16, 13, 55,DateTimeKind.Utc),
                Mbid = "79f3dc97-2297-47ee-8556-9a1bb4b48d53",
                Name = "Pinball Wizard",
                ArtistMbid = "9fdaa16b-a6c4-4831-b87c-bc9ca8ce7eaa",
                AlbumName = "Tommy (Remastered)",
                Url = new Uri("http://www.last.fm/music/The+Who/_/Pinball+Wizard", UriKind.Absolute),
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/64s/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/126/35234991.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/35234991.jpg")
            };

            await CompareResultsMultiple(_command, expectedTrack, UserApiResponses.UserGetRecentTracksMultiple, 2);
        }

        [TestMethod]
        public async Task HandleResponseSingle()
        {
            var expectedTrack = new LastTrack
            {
                ArtistName = "Rick James",
                Mbid = "",
                Name = "Super Freak (Part 1) - 1981 Single Version",
                ArtistMbid = "cba9cec2-be8d-41bd-91b4-a1cd7de39b0c",

                TimePlayed = new DateTime(2014,12,20,10,16,52, DateTimeKind.Utc),
                AlbumName = "The Definitive Collection",
                Url = new Uri("http://www.last.fm/music/Rick+James/_/Super+Freak+(Part+1)+-+1981+Single+Version", UriKind.Absolute),
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/64s/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/126/90462319.jpg",
                    "http://userserve-ak.last.fm/serve/300x300/90462319.jpg")
            };

            var expected = new List<LastTrack> { expectedTrack };

            await CompareResultsSingle(_command, expected, UserApiResponses.UserGetRecentTracksSingle);
        }

        [TestMethod]
        public async Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksError));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Error == LastFmApiError.MissingParameters);
        }
    }
}
