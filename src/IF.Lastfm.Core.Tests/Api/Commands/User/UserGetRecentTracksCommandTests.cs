using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetRecentTracksCommandTests : CommandTestsBase
    {
        [Test]
        public async Task HandleResponseMultiple()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };

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

            var file = GetFileContents("UserApi.UserGetRecentTracksMultiple.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksMultiple));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedTrack, actual.Content[2]);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };

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
            
            var file = GetFileContents("UserApi.UserGetRecentTracksSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksSingle));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedTrack, actual.Single());
        }

        [Test]
        public async Task HandleExtendedResponse()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1,
                Extended = true
            };

            var expectedTrack = new LastTrack
            {
                ArtistName = "Republika",
                ArtistMbid = "116a9ec8-148e-4b3d-8fb9-3d995cc4159e",
                ArtistUrl = new Uri("https://www.last.fm/music/Republika", UriKind.Absolute),
                ArtistImages = new LastImageSet(
                    "https://lastfm-img2.akamaized.net/i/u/34s/e45f11a32d134ed4aeb1ce7b25445fb2.png",
                    "https://lastfm-img2.akamaized.net/i/u/64s/e45f11a32d134ed4aeb1ce7b25445fb2.png",
                    "https://lastfm-img2.akamaized.net/i/u/174s/e45f11a32d134ed4aeb1ce7b25445fb2.png",
                    "https://lastfm-img2.akamaized.net/i/u/300x300/e45f11a32d134ed4aeb1ce7b25445fb2.png"),
                TimePlayed = new DateTime(2018, 06, 27, 16, 32, 16, DateTimeKind.Utc),
                Mbid = string.Empty,
                Name = "Tak Długo Czekam (Ciało) (live)",
                AlbumName = "Koncerty w Trójce - Republika",
                Url = new Uri("https://www.last.fm/music/Republika/_/Tak+D%C5%82ugo+Czekam+(Cia%C5%82o)+(live)", UriKind.Absolute),
                Images = new LastImageSet(
                    "https://lastfm-img2.akamaized.net/i/u/34s/1756f85e7332d469dd17b2e1e0a4d16c.png",
                    "https://lastfm-img2.akamaized.net/i/u/64s/1756f85e7332d469dd17b2e1e0a4d16c.png",
                    "https://lastfm-img2.akamaized.net/i/u/174s/1756f85e7332d469dd17b2e1e0a4d16c.png",
                    "https://lastfm-img2.akamaized.net/i/u/300x300/1756f85e7332d469dd17b2e1e0a4d16c.png"),
                IsLoved = true

            };

            var file = GetFileContents("UserApi.UserGetRecentTracksExtended.json");
            var response = CreateResponseMessage(file);

            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedTrack, actual.Single());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var command = new GetRecentTracksCommand(MAuth.Object, "rj")
            {
                Count = 1
            };

            var file = GetFileContents("UserApi.UserGetRecentTracksError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksError));

            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
