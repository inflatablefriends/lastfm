using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    public abstract class LastAsyncCommandBase<T> : IAsyncCommand<T>
    {
        public string Method { get; protected set; }
        public Uri Url { get; protected set; }
        public IAuth Auth { get; protected set; }
        public int Page { get; set; }
        public int Count { get; set; }

        protected Dictionary<string, string> Parameters { get; set; }

        protected LastAsyncCommandBase()
        {
            Parameters = new Dictionary<string, string>();
        } 

        public abstract void SetParameters();
        protected abstract Uri BuildRequestUrl();
        public abstract Task<T> ExecuteAsync();
        public abstract Task<T> HandleResponse(HttpResponseMessage response);

        protected void AddPagingParameters()
        {
            Parameters.Add("page", Page.ToString());
            Parameters.Add("limit", Count.ToString());
        }

        /// <summary>
        /// Annoying workaround for Windows Phone's caching... 
        /// see http://stackoverflow.com/questions/6334788/windows-phone-7-webrequest-caching
        /// </summary>
        protected void DisableCaching()
        {
            Parameters.Add("disablecachetoken", DateTime.UtcNow.Ticks.ToString());
        }
    }
}