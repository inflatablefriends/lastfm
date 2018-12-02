using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    using System.Web.Configuration;

    class UserGetLovedTracksCommandTests : CommandTestsBase
    {
        [Test]
        public async Task HandleResponseMultiple()
        {
            var command = new GetLovedTracksCommand(MAuth.Object, "rj")
                              {
                                  Count = 3
                              };

            var expectedTrack = new LastTrack
                                    {
                                        ArtistName = "The Rolling Stones",
                                        TimePlayed = new DateTime(2014, 12, 19, 16, 16, 56, DateTimeKind.Utc),
                                        Mbid = "3dde65c8-22c3-4637-b67e-b234177c847b",
                                        Name = "Love in Vain",
                                        ArtistMbid = "b071f9fa-14b0-4217-8e97-eb41da73f598",
                                        AlbumName = "Let It Bleed",
                                        Url = new Uri("http://www.last.fm/music/The+Rolling+Stones/_/Love+in+Vain"),
                                        Images = new LastImageSet(
                                            "http://userserve-ak.last.fm/serve/34s/101739037.png",
                                            "http://userserve-ak.last.fm/serve/64s/101739037.png",
                                            "http://userserve-ak.last.fm/serve/126/101739037.png",
                                            "http://userserve-ak.last.fm/serve/300x300/101739037.png")
                                    };
            var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetLovedTracksMultiple));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedTrack, actual.Content[1]);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var command = new GetLovedTracksCommand(MAuth.Object, "rj")
                              {
                                  Count = 1
                              };
            var expectedTrack = new LastTrack
                                    {
                                        ArtistName = "Rick James",
                                        Mbid = "",
                                        Name = "Super Freak (Part 1) - 1981 Single Version",
                                        ArtistMbid = "cba9cec2-be8d-41bd-91b4-a1cd7de39b0c",

                                        TimePlayed = new DateTime(2014, 12, 20, 10, 16, 52, DateTimeKind.Utc),
                                        AlbumName = "The Definitive Collection",
                                        Url = new Uri("http://www.last.fm/music/Rick+James/_/Super+Freak+(Part+1)+-+1981+Single+Version", UriKind.Absolute),
                                        Images = new LastImageSet(
                                            "http://userserve-ak.last.fm/serve/34s/90462319.jpg",
                                            "http://userserve-ak.last.fm/serve/64s/90462319.jpg",
                                            "http://userserve-ak.last.fm/serve/126/90462319.jpg",
                                            "http://userserve-ak.last.fm/serve/300x300/90462319.jpg")
                                    };
            var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetLovedTracksSingle));
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
            var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecentTracksError));
            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
