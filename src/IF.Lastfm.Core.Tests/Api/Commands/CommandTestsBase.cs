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
using System.Threading.Tasks;

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

        public void CheckResult(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
            var actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));


        }

        public async Task CheckResult_Single(GetAsyncCommandBase<PageResponse<LastTrack>> _command, object expected, byte[] Data) 
        {


            var response = CreateResponseMessage(Encoding.UTF8.GetString(Data));
            //Check if object not array
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var actual = parsed.Content;

            CheckResult(expected, actual);

        }

        public async Task CheckResult_MultipleSample(GetAsyncCommandBase<PageResponse<LastTrack>> _command, object expected, int arrayID, byte[] Data) 
        {


            var response = CreateResponseMessage(Encoding.UTF8.GetString(Data));
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var actual = parsed.Content[arrayID];

            CheckResult(expected, actual);

        }
    }
}
