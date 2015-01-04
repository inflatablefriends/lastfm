using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands
{
    internal abstract class UnauthenticatedPostAsyncCommandBase<T> : PostAsyncCommandBase<T> where T : LastResponse, new()
    {
        protected UnauthenticatedPostAsyncCommandBase(ILastAuth auth) : base(auth)
        {
        }

        public override Task<T> ExecuteAsync()
        {
            return ExecuteAsyncInternal();
        }
    }
}