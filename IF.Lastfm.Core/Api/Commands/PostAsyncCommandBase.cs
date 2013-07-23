using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    internal abstract class PostAsyncCommandBase<T>
    {
        public string Method { get; protected set; }
        public Uri Url { get; protected set; }
        public IAuth Auth { get; protected set; }

        public int Page { get; set; }
        public int Count { get; set; }

        protected PostAsyncCommandBase(IAuth auth)
        {
            Auth = auth;
            Url = new Uri(LastFm.ApiRoot, UriKind.Absolute);
        }

        public abstract Task<T> ExecuteAsync();

        protected async Task<T> ExecuteInternal(Dictionary<string, string> parameters)
        {
            var apisig = Auth.GenerateMethodSignature(Method, parameters);

            var postContent = LastFm.CreatePostBody(Method,
                Auth.ApiKey,
                apisig,
                parameters);

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(Url, postContent);
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