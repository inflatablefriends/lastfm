using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using Moq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public abstract class CommandTestsBase
    {
        public Mock<IAuth> MAuth { get; private set; }

        protected CommandTestsBase()
        {
            MAuth = new Mock<IAuth>();
        }

        public abstract void Constructor();
        public abstract Task HandleSuccessResponse();

        protected HttpResponseMessage CreateResponseMessage(string message)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
                           {
                               Content = new StringContent(message, Encoding.UTF8)
                           };

            return response;
        }
    }
}
