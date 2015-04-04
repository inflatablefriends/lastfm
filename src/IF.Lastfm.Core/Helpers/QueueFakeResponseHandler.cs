using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Helpers
{
    public class QueueFakeResponseHandler : DelegatingHandler
    {
        private readonly Queue<HttpResponseMessage> _queuedResponses;

        public QueueFakeResponseHandler()
        {
            _queuedResponses = new Queue<HttpResponseMessage>();
        }

        public void Enqueue(HttpResponseMessage message)
        {
            _queuedResponses.Enqueue(message);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = _queuedResponses.Dequeue()
                           ?? new HttpResponseMessage(HttpStatusCode.NotFound) {RequestMessage = request};

            return Task.FromResult(response);
        }
    }
}