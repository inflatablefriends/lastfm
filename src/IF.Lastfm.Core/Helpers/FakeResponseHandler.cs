using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Helpers
{
    /// <summary>
    /// http://stackoverflow.com/a/22264503/268555
    /// </summary>
    public class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _fakeResponses = new Dictionary<Uri, HttpResponseMessage>();

        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _fakeResponses.Add(uri, responseMessage);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            if (_fakeResponses.ContainsKey(request.RequestUri))
            {
                response = _fakeResponses[request.RequestUri];
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request };
            }

            return Task.FromResult(response);
        }
    }
}
