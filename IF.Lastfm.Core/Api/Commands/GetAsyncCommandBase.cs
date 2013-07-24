using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    internal abstract class GetAsyncCommandBase<T> : IAsyncCommand<T>
    {
        public string Method { get; protected set; }
        public Uri Url { get; protected set; }
        public IAuth Auth { get; protected set; }

        public int Page { get; set; }
        public int Count { get; set; }

        protected GetAsyncCommandBase(IAuth auth)
        {
            Auth = auth;
        }

        public abstract Uri BuildRequestUrl();

        public async Task<T> ExecuteAsync()
        {
            Url = BuildRequestUrl();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(Url);
            return await HandleResponse(response);
        }

        public abstract Task<T> HandleResponse(HttpResponseMessage response);

        protected void AddPagingParameters(Dictionary<string, string> parameters)
        {
            parameters.Add("page", Page.ToString());
            parameters.Add("limit", Count.ToString());
        }

        /// <summary>
        /// Annoying workaround for Windows Phone's caching... 
        /// see http://stackoverflow.com/questions/6334788/windows-phone-7-webrequest-caching
        /// </summary>
        /// <param name="parameters"></param>
        protected void DisableCaching(Dictionary<string, string> parameters)
        {
            parameters.Add("disablecachetoken", DateTime.UtcNow.Ticks.ToString());
        }
    }
}