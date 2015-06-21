using IF.Lastfm.Core.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

namespace IF.Lastfm.Core.Api.Commands
{
    /// <summary>
    /// Having this type makes reflection easier - there probably isn't any other need for it
    /// </summary>
    public abstract class LastAsyncCommandBase
    {
        private string _methodName;

        public string Method
        {
            get
            {
                if (!String.IsNullOrEmpty(_methodName))
                {
                    return _methodName;
                }

                var methodNameAttribute = this.GetType().GetTypeInfo().GetCustomAttribute<ApiMethodNameAttribute>();
                if (methodNameAttribute == null)
                {
                    throw new NotImplementedException(@"Could not find an ApiMethodNameAttribute on the current Command implementation.
This custom attribute must be present on all Commands. For more information, see the ApiMethodNameAttribute documentation.");
                }
                return methodNameAttribute.Text;
            }
            internal set { _methodName = value; }
        }

        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// The HttpClient used for the request. 
        /// </summary>
        public HttpClient HttpClient { get; set; }
    }

    public abstract class LastAsyncCommandBase<T> : LastAsyncCommandBase, IAsyncCommand<T> where T : LastResponse, new()
    {
        public ILastAuth Auth { get; protected set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public Uri Url { get; protected set; }

        protected LastAsyncCommandBase()
        {
            Parameters = new Dictionary<string, string>();
        }

        public abstract void SetParameters();

        protected abstract Uri BuildRequestUrl();

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

        public abstract Task<T> ExecuteAsync();

        public abstract Task<T> HandleResponse(HttpResponseMessage response);
    }
}