using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    public abstract class GetAsyncCommandBase<T> : LastAsyncCommandBase<T>
    {           
        protected GetAsyncCommandBase(IAuth auth)
        {
            Auth = auth;
        }

        public async override Task<T> ExecuteAsync()
        {
            SetParameters();

            EscapeParameters();

            Url = BuildRequestUrl();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(Url);
            return await HandleResponse(response);
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