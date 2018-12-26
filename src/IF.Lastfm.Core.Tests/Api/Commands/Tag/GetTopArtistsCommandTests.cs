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
    public class GetTopArtistsCommandTests: CommandTestsBase
    {
        [Test]
        public async Task HandleSingleResultResponse()
        {
            //Arrange
            GetTopArtistsCommand command = new GetTopArtistsCommand(MAuth.Object, "disco");

            //Act
            var file = GetFileContents("Tag.GetTopArtistsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetTopArtistsSingle));
            var parsed = await command.HandleResponse(response);

            //Assert
            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            //Arrange
            GetTopArtistsCommand command = new GetTopArtistsCommand(MAuth.Object, "disco");

            //Act
            var file = GetFileContents("Tag.GetTopArtistsSuccess.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetTopArtistsSuccess));
            var parsed = await command.HandleResponse(response);

            //Assert
            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
        }
        
        [Test]
        public async Task ErrorIfTagIsEmpty()
        {
            var command = new GetTopArtistsCommand(MAuth.Object, "errorTag");
            
            var file = GetFileContents("Tag.GetTopArtistsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetTopArtistsError));

            var parsed = await command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
