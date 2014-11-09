using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using System;
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

        public override async Task<T> ExecuteAsync()
        {
            SetParameters();

            Url = BuildRequestUrl();

            if (Auth.Authenticated)
            {
                Parameters.Add("sk", Auth.UserSession.Token);
            }

            var apisig = Auth.GenerateMethodSignature(Method, Parameters);

            var postContent = LastFm.CreatePostBody(Method,
                Auth.ApiKey,
                apisig,
                Parameters);

            try
            {
                var httpClient = GetHttpClient();
                var response = await httpClient.PostAsync(Url, postContent);
                return await HandleResponse(response);
            }
            catch (HttpRequestException)
            {
                if (LastFm.CatchRequestExceptions)
                {
                    return LastResponse.CreateErrorResponse<T>(LastFmApiError.RequestFailed);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}