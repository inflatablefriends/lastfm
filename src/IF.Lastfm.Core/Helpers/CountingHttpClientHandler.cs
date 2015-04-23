using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Helpers
{
    public class CountingHttpClientHandler : HttpClientHandler
    {
        public int Count { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Count++;
            return base.SendAsync(request, cancellationToken);
        }
    }
}