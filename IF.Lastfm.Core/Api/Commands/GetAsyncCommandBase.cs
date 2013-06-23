using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    internal abstract class GetAsyncCommandBase<T> : IAsyncLastCommand<T>
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

        public abstract Task<T> ExecuteAsync();

        protected async Task<T> ExecuteInternal()
        {
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
    }
}