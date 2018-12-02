using System;
using System.Linq;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Tag;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Tests.Api.Commands.Tag
{
    public class GetSimilarCommandTests : CommandTestsBase
    {
        [Test]
        public async Task HandleSuccessResponse()
        {
            const string tagName = "daria";
            var command = new GetSimilarCommand(MAuth.Object, tagName);

            var expectedTagNames = new[]
            {
                "road trip",
                "longing",
                "old school rap",
                "pj harvey",
                "girl band",
                "alt rock",
                "female rock",
                "90s",
                "post-grunge",
                "acid jazz"
            };
            var expectedTags = expectedTagNames.Select(tag => new LastTag
            {
                Name = tag,
                Url = new Uri(String.Format("http://www.last.fm/tag/{0}", Uri.EscapeUriString(tag))),
                RelatedTo = tagName,
                Streamable = true
            }).ToList();

            expectedTags[0].Streamable = false;
            expectedTags[1].Streamable = null;
            
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetSimilarSuccess));
            var actual = await command.HandleResponse(response);

            Assert.IsTrue(actual.Skip(2).All(t => t.Streamable.GetValueOrDefault()));
            Assert.IsTrue(actual.All(t => t.RelatedTo == tagName));
            Assert.IsTrue(actual.Success);
            TestHelper.AssertSerialiseEqual(expectedTags, actual.ToList());
        }
        
        [Test]
        public async Task HandleErrorResponse()
        {
            var command = new GetSimilarCommand(MAuth.Object, "arroooo");

            var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetSimilarError));

            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
