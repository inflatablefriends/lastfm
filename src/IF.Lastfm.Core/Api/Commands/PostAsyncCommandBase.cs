using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    public abstract class PostAsyncCommandBase<T> : LastAsyncCommandBase<T> where T : LastResponse, new()
    {
        protected PostAsyncCommandBase(ILastAuth auth)
        {
            Auth = auth;
        }

        protected override Uri BuildRequestUrl()
        {
            return new Uri(LastFm.ApiRoot, UriKind.Absolute);
        }

        public override Task<T> ExecuteAsync()
        {
            if (!Auth.Authenticated)
            {
                return Task.FromResult(LastResponse.CreateErrorResponse<T>(LastResponseStatus.BadAuth));
            }

            return ExecuteAsyncInternal();
        }

        protected async Task<T> ExecuteAsyncInternal()
        {
            SetParameters();

            var toRemove = Parameters.Where(p => String.IsNullOrEmpty(p.Value)).ToList();
            foreach (var parameter in toRemove)
            {
                Parameters.Remove(parameter.Key);
            }

            Url = BuildRequestUrl();

            var apisig = Auth.GenerateMethodSignature(Method, Parameters);

            var postContent = LastFm.CreatePostBody(Method,
                Auth.ApiKey,
                apisig,
                Parameters);

            try
            {
                var httpClient = HttpClient;
                using (var response = await httpClient.PostAsync(Url, postContent))
                {
                    return await HandleResponse(response);
                }
            }
            catch (HttpRequestException)
            {
                return LastResponse.CreateErrorResponse<T>(LastResponseStatus.RequestFailed);
            }
        }
    }
}