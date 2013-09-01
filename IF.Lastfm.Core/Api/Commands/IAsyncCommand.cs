using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands
{
    internal interface IAsyncCommand<T> where T : LastResponse, new()
    {
        Task<T> ExecuteAsync();
    }
}
