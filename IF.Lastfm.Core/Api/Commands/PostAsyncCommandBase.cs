using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    internal abstract class PostAsyncCommandBase<T> : LastAsyncCommandBase<T>
    {
        protected PostAsyncCommandBase(IAuth auth)
        {
            Auth = auth;
        }

        protected override Uri BuildRequestUrl()
        {
            return new Uri(LastFm.ApiRoot, UriKind.Absolute);
        }

        public override async Task<T> ExecuteAsync()
        {
            SetParameters();

            Url = BuildRequestUrl();

            Parameters.Add("sk", Auth.User.Token);

            var apisig = Auth.GenerateMethodSignature(Method, Parameters);

            var postContent = LastFm.CreatePostBody(Method,
                Auth.ApiKey,
                apisig,
                Parameters);

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(Url, postContent);
            return await HandleResponse(response);
        }
    }
}