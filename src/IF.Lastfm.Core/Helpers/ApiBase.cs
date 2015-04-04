using System.Net.Http;

namespace IF.Lastfm.Core.Helpers
{
    public abstract class ApiBase
    {
        protected HttpClient HttpClient { get; private set; }

        protected ApiBase(HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            HttpClient = httpClient;
        }
    }
}