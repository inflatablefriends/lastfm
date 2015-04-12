using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Helpers
{
    public class QueueFakeResponseHandler : DelegatingHandler
    {
        private readonly Queue<HttpResponseMessage> _queuedResponses;

        public List<Tuple<HttpRequestMessage, string>> Sent { get; private set; }

        public QueueFakeResponseHandler()
        {
            _queuedResponses = new Queue<HttpResponseMessage>();
            Sent = new List<Tuple<HttpRequestMessage, string>>();
        }

        public void Enqueue(HttpResponseMessage message)
        {
            _queuedResponses.Enqueue(message);
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Sent.Add(Tuple.Create(request, await request.Content.ReadAsStringAsync()));
            var response = _queuedResponses.Dequeue()
                           ?? new HttpResponseMessage(HttpStatusCode.NotFound) {RequestMessage = request};

            return response;
        }
    }
}