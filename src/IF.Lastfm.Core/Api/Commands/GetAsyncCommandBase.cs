using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    public abstract class GetAsyncCommandBase<T> : LastAsyncCommandBase<T> where T : LastResponse, new()
    {           
        protected GetAsyncCommandBase(ILastAuth auth)
        {
            Auth = auth;
        }

        public async override Task<T> ExecuteAsync()
        {
            SetParameters();

            EscapeParameters();

            Url = BuildRequestUrl();

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(Url);
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

        protected override Uri BuildRequestUrl()
        {
            var apiUrl = LastFm.FormatApiUrl(Method, Auth.ApiKey, Parameters);
            return new Uri(apiUrl, UriKind.Absolute);
        }

        private void EscapeParameters()
        {
            foreach (var key in Parameters.Keys.ToList())
            {
                Parameters[key] = Uri.EscapeDataString(Parameters[key]);
            }
        }
    }
}