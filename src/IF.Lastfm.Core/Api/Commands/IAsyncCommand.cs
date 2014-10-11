using IF.Lastfm.Core.Api.Helpers;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    internal interface IAsyncCommand<T> where T : LastResponse, new()
    {
        Task<T> ExecuteAsync();
    }
}
