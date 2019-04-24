using System;
using System.Collections.Generic;
using System.Linq;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Album;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.AlbumApi
{
    public class GetAlbumInfoCommandTests : CommandTestsBase
    {
        private GetInfoCommand _command;

        public GetAlbumInfoCommandTests()
        {
            _command = new GetInfoCommand(MAuth.Object)
            {
                AlbumName = "Ray of Light",
                ArtistName = "Madonna"
            };

            _command.SetParameters();
        }


        [Test]
        public async Task HandleSuccessResponse()
        {
            var expectedAlbum = new LastAlbum
            {
                ArtistName = "Madonna",
                ListenerCount = 509271,
                PlayCount = 7341494,
                Mbid = "ddb3168d-66a9-4b2d-af02-05278da8a23d",
                Url = new Uri("http://www.last.fm/music/Madonna/Ray+of+Light", UriKind.Absolute),
                Name = "Ray of Light",
                ReleaseDateUtc = new DateTime(2005, 09, 13, 0, 0, 0),
                Id = "1934",
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/37498173.png",
                    "http://userserve-ak.last.fm/serve/64s/37498173.png",
                    "http://userserve-ak.last.fm/serve/174s/37498173.png",
                    "http://userserve-ak.last.fm/serve/300x300/37498173.png",
                    "http://userserve-ak.last.fm/serve/_/37498173/Ray+of+Light.png"),
                TopTags = new List<LastTag>
                {
                    new LastTag("albums i own", "http://www.last.fm/tag/albums%20i%20own"),
                    new LastTag("pop", "http://www.last.fm/tag/pop"),
                    new LastTag("electronic", "http://www.last.fm/tag/electronic"),
                    new LastTag("dance", "http://www.last.fm/tag/dance"),
                    new LastTag("madonna", "http://www.last.fm/tag/madonna")
                },
                Wiki = new LastWiki() {
                    Published = new DateTime(2012, 07, 14, 14, 33, 01 ),
                    Summary = "Ray of Light is the seventh studio album by American singer-songwriter Madonna, released on March 3, 1998 by Maverick Records. The album is a pop and dance record, yet, it contains elements of electronic music within its composition, making it a departure from her previous work. Additionally, it incorporates several genres and subgenres, including techno and ambient. The album has several themes, including spirituality.",
                    Content = "Ray of Light is the seventh studio album by American singer-songwriter Madonna, released on March 3, 1998 by Maverick Records. The album is a pop and dance record, yet, it contains elements of electronic music within its composition, making it a departure from her previous work. Additionally, it incorporates several genres and subgenres, including techno and ambient. The album has several themes, including spirituality. \n \n After giving birth to her daughter Lourdes, Madonna collaborated with Patrick Leonard and William Orbit in developing the album. After failed sessions with other producers, Madonna pursued a new musical direction with Orbit and incorporated his extensive usage of trance and electronic music in her songs. The recording took place over four months, but experienced problems with the Pro Tools arrangement by Orbit as well as the absence of live bands.\n \n However, upon release, the album was lauded by contemporary critics as a music masterpiece of the decade. Reviewers complimented the album for its mature, restrained nature as well as commending Madonna's musical direction, calling it her &quot;most adventurous&quot; record. In such a way, the album has been included in many critic lists and polls, including Rolling Stone magazine's &quot;The 500 Greatest Albums of All Time&quot;.\n \n Commercially, the album was a success on the world charts, peaking at number one in countries like the United Kingdom, Canada, Australia, New Zealand and mainland Europe. On the U.S. Billboard 200, the album debuted and peaked at number two. The Recording Industry Association of America (RIAA) certified it Quadruple Platinum on March 16, 2000, recognizing four million shipments in the United States, making it her fifth best-selling recording there. Worldwide sales of the album have surpassed 20 million copies.\n \n Five singles were released from the album. The first single &quot;Frozen&quot; was an international success, as was the second one, &quot;Ray of Light&quot;, which won a number of awards for its music video. In 1999, the album received three Grammy Awards, including &quot;Best Pop Vocal Album&quot;, and &quot;Best Dance Recording.&quot; In 2003, the album was ranked #363 on Rolling Stone's 500 Greatest Albums of All Time. Madonna has performed songs from this album on a number of her world tours, most recently the Sticky &amp; Sweet Tour (2008-09).\n        \nUser-contributed text is available under the Creative Commons By-SA License and may also be available under the GNU FDL."
                }
            };

            var file = GetFileContents("AlbumApi.AlbumGetInfoSuccess.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetInfoSuccess));
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var actual = parsed.Content;
            Assert.IsTrue(actual.Tracks.Count() == 13);
            actual.Tracks = null;

            var expectedJson = JsonConvert.SerializeObject(expectedAlbum, Formatting.Indented);
            var actualJson = JsonConvert.SerializeObject(parsed.Content, Formatting.Indented);

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task HandleSuccessResponseForUser()
        {
            _command = new GetInfoCommand(MAuth.Object)
            {
                AlbumName = "Ray of Light",
                ArtistName = "Madonna",
                UserName = "user"
            };

            var expectedAlbum = new LastAlbum
            {
                ArtistName = "Madonna",
                ListenerCount = 509271,
                PlayCount = 7341494,
                UserPlayCount = 321,
                Mbid = "ddb3168d-66a9-4b2d-af02-05278da8a23d",
                Url = new Uri("http://www.last.fm/music/Madonna/Ray+of+Light", UriKind.Absolute),
                Name = "Ray of Light",
                ReleaseDateUtc = new DateTime(2005, 09, 13, 0, 0, 0),
                Id = "1934",
                Images = new LastImageSet(
                    "http://userserve-ak.last.fm/serve/34s/37498173.png",
                    "http://userserve-ak.last.fm/serve/64s/37498173.png",
                    "http://userserve-ak.last.fm/serve/174s/37498173.png",
                    "http://userserve-ak.last.fm/serve/300x300/37498173.png",
                    "http://userserve-ak.last.fm/serve/_/37498173/Ray+of+Light.png"),
                TopTags = new List<LastTag>
                {
                    new LastTag("albums i own", "http://www.last.fm/tag/albums%20i%20own"),
                    new LastTag("pop", "http://www.last.fm/tag/pop"),
                    new LastTag("electronic", "http://www.last.fm/tag/electronic"),
                    new LastTag("dance", "http://www.last.fm/tag/dance"),
                    new LastTag("madonna", "http://www.last.fm/tag/madonna")
                },
                Wiki = new LastWiki() {
                    Published = new DateTime(2012, 07, 14, 14, 33, 01),
                    Summary = "Ray of Light is the seventh studio album by American singer-songwriter Madonna, released on March 3, 1998 by Maverick Records. The album is a pop and dance record, yet, it contains elements of electronic music within its composition, making it a departure from her previous work. Additionally, it incorporates several genres and subgenres, including techno and ambient. The album has several themes, including spirituality.",
                    Content = "Ray of Light is the seventh studio album by American singer-songwriter Madonna, released on March 3, 1998 by Maverick Records. The album is a pop and dance record, yet, it contains elements of electronic music within its composition, making it a departure from her previous work. Additionally, it incorporates several genres and subgenres, including techno and ambient. The album has several themes, including spirituality. \n \n After giving birth to her daughter Lourdes, Madonna collaborated with Patrick Leonard and William Orbit in developing the album. After failed sessions with other producers, Madonna pursued a new musical direction with Orbit and incorporated his extensive usage of trance and electronic music in her songs. The recording took place over four months, but experienced problems with the Pro Tools arrangement by Orbit as well as the absence of live bands.\n \n However, upon release, the album was lauded by contemporary critics as a music masterpiece of the decade. Reviewers complimented the album for its mature, restrained nature as well as commending Madonna's musical direction, calling it her &quot;most adventurous&quot; record. In such a way, the album has been included in many critic lists and polls, including Rolling Stone magazine's &quot;The 500 Greatest Albums of All Time&quot;.\n \n Commercially, the album was a success on the world charts, peaking at number one in countries like the United Kingdom, Canada, Australia, New Zealand and mainland Europe. On the U.S. Billboard 200, the album debuted and peaked at number two. The Recording Industry Association of America (RIAA) certified it Quadruple Platinum on March 16, 2000, recognizing four million shipments in the United States, making it her fifth best-selling recording there. Worldwide sales of the album have surpassed 20 million copies.\n \n Five singles were released from the album. The first single &quot;Frozen&quot; was an international success, as was the second one, &quot;Ray of Light&quot;, which won a number of awards for its music video. In 1999, the album received three Grammy Awards, including &quot;Best Pop Vocal Album&quot;, and &quot;Best Dance Recording.&quot; In 2003, the album was ranked #363 on Rolling Stone's 500 Greatest Albums of All Time. Madonna has performed songs from this album on a number of her world tours, most recently the Sticky &amp; Sweet Tour (2008-09).\n        \nUser-contributed text is available under the Creative Commons By-SA License and may also be available under the GNU FDL."
                }

            };

            var file = GetFileContents("AlbumApi.AlbumGetInfoForUser.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetInfoForUser));
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var actual = parsed.Content;
            Assert.IsTrue(actual.Tracks.Count() == 13);
            actual.Tracks = null;

            var expectedJson = JsonConvert.SerializeObject(expectedAlbum, Formatting.Indented);
            var actualJson = JsonConvert.SerializeObject(parsed.Content, Formatting.Indented);

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetInfoMissing.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetInfoMissing));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
