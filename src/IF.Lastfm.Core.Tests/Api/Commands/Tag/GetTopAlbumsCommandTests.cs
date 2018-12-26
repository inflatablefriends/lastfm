using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Tag;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Tag
{
    public class GetTopAlbumsCommandTests: CommandTestsBase
    {
        [Test]
        public async Task HandleSingleResultResponse()
        {
            //Arrange
            GetTopAlbumsCommand command = new GetTopAlbumsCommand(MAuth.Object, "disco");

            //Act
            var file = GetFileContents("Tag.GetTopAlbumsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetTopAlbumsSingle));
            var parsed = await command.HandleResponse(response);

            //Assert
            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            //Arrange
            GetTopAlbumsCommand command = new GetTopAlbumsCommand(MAuth.Object, "disco");

            //Act
            var file = GetFileContents("Tag.GetTopAlbumsSuccess.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetTopAlbumsSuccess));
            var parsed = await command.HandleResponse(response);

            //Assert
            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var command = new GetTopAlbumsCommand(MAuth.Object, "errorTag");

            var file = GetFileContents("Tag.GetTopAlbumsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetTopAlbumsError));

            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
