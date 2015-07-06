using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface ITagApi
    {
        Task<PageResponse<LastTag>> GetSimilarAsync(string tagName);
        Task<LastResponse<LastTag>> GetInfoAsync(string tagName);
    }
}