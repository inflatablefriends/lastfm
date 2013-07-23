using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    internal interface IAsyncCommand<T>
    {
        Uri BuildRequestUrl();
        Task<T> ExecuteAsync();
        Task<T> HandleResponse(HttpResponseMessage response);
    }
}
