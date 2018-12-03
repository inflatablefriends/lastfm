using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Tag;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Tag
{
    public class GetInfoCommandTests: CommandTestsBase
    {
        [Test]
        public async Task HandleSuccessResponse()
        {
            //Arrange
            const string tagName = "disco";
            const string tagUri = "http://www.last.fm/tag/disco";

            var command = new GetInfoCommand(MAuth.Object, tagName);
            var expectedTag=new LastTag(tagName,tagUri)
            {
                Reach = 34671,
                Count = 172224,
                Streamable = true
            };


            //Act
            var file = GetFileContents("Tag.GetInfoSuccess.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetInfoSuccess));
            var lastResponse = await command.HandleResponse(response);
            var tag = lastResponse.Content;

            //Assert
            Assert.IsTrue(lastResponse.Success);
            Assert.AreEqual(expectedTag.Reach,tag.Reach);
            Assert.AreEqual(expectedTag.Name, tag.Name);
            Assert.AreEqual(expectedTag.Count, tag.Count);
            Assert.AreEqual(expectedTag.Streamable, tag.Streamable);
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var command = new GetInfoCommand(MAuth.Object, "errorTag");

            var file = GetFileContents("Tag.GetInfoError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetInfoError));

            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
