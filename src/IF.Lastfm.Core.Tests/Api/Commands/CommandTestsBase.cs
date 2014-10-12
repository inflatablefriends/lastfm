using IF.Lastfm.Core.Api;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        public abstract Task HandleResponseSingle();
        public abstract Task HandleEmptyResponse();
        public abstract Task HandleErrorResponse();

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
