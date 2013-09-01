using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands
{
    internal interface IAsyncCommand<T>
    {
        Task<T> ExecuteAsync();
    }
}
