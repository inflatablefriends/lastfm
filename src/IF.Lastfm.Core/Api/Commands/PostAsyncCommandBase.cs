using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands
{
    public abstract class PostAsyncCommandBase<T> : LastAsyncCommandBase<T> where T : LastResponse, new()
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

            if (Auth.HasAuthenticated)
            {
                Parameters.Add("sk", Auth.User.Token);
            }

            var apisig = Auth.GenerateMethodSignature(Method, Parameters);

            var postContent = LastFm.CreatePostBody(Method,
                Auth.ApiKey,
                apisig,
                Parameters);

            try
            {
                var httpClient = new HttpClient();
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