using System;
using System.Net.Http;

namespace IF.Lastfm.Core.Helpers
{
    public abstract class ApiBase : IDisposable
    {
        private readonly bool _isHttpClientOwner;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// The HttpClient that will be used by this API. If it is provided through the ApiBase constructor then it should be disposed explicitly.
        /// </summary>
        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }

        protected ApiBase(HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();

                // See http://stackoverflow.com/questions/14595021/how-to-disable-the-expect-100-continue-header-in-winrts-httpwebrequest
                httpClient.DefaultRequestHeaders.ExpectContinue = false;

                _isHttpClientOwner = true;
            }

            _httpClient = httpClient;
        }

        public virtual void Dispose()
        {
            if (_isHttpClientOwner)
            {
                HttpClient.Dispose();
            }
        }
    }
}