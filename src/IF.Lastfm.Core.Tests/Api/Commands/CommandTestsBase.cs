using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public abstract class CommandTestsBase
    {
        public Mock<ILastAuth> MAuth { get; private set; }

        protected CommandTestsBase()
        {
            MAuth = new Mock<ILastAuth>();
        }
        
        protected HttpResponseMessage CreateResponseMessage(string message)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
                           {
                               Content = new StringContent(message, Encoding.UTF8)
                           };

            return response;
        }

        private static void JsonCompare(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
            var actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        protected async Task CompareResultsSingle(GetAsyncCommandBase<PageResponse<LastTrack>> command, object expected, byte[] resource) 
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(resource));

            var parsed = await command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            var actual = parsed.Content;
            JsonCompare(expected, actual);
        }

        protected async Task CompareResultsMultiple(GetAsyncCommandBase<PageResponse<LastTrack>> command, object expected, byte[] resource, int itemIndex) 
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(resource));

            var parsed = await command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            var actual = parsed.Content[itemIndex];
            JsonCompare(expected, actual);
        }
    }
}
