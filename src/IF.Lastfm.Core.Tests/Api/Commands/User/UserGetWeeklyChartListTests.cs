using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.User;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using Moq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetWeeklyChartListsTests : CommandTestsBase
    {
        private const string user = "test";

        private GetWeeklyChartsCommand _command;
        private Mock<ILastAuth> _mockAuth;

        [SetUp]
        public void TestInitialise()
        {
            _mockAuth = new Mock<ILastAuth>();
            _command = new GetWeeklyChartsCommand(_mockAuth.Object, user)
            {
                //no parameters
             };

            _command.SetParameters();
        }

        [Test]
        public void CorrectParameters()
        {
            var expected = new Dictionary<string, string>
            {
                {"user", user},
                {"disablecachetoken", ""}
            };

            _command.Parameters["disablecachetoken"] = "";

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            //reusing the error message file
            var file = GetFileContents("UserApi.UserGetTopAlbumsError.json");
            var response = CreateResponseMessage(file);
            //var http = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopAlbumsError));
            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.AreEqual(LastResponseStatus.MissingParameters, parsed.Status);
        }

        [Test]
        public async Task GetWeeklyChartList_HandleResponse_Success()
        {
            var file = GetFileContents("UserApi.UserGetWeeklyChartList.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);
            
            Assert.IsTrue(parsed.Success);
            Assert.AreEqual(612, parsed.Content.Count);

            //convert dates back to unix time
            var lastPeriod = parsed.Content.Last();
            var dto_from = new DateTimeOffset(lastPeriod.From, TimeSpan.Zero);
            var dto_to = new DateTimeOffset(lastPeriod.To, TimeSpan.Zero);

            Assert.AreEqual("1546171200", dto_from.ToUnixTimeSeconds().ToString());
            Assert.AreEqual("1546776000", dto_to.ToUnixTimeSeconds().ToString());
        }

    }
}