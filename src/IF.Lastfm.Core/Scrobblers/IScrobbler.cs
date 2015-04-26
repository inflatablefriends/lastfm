using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Scrobblers
{
    public interface IScrobbler
    {
        bool CacheEnabled { get; }

        Task<IEnumerable<Scrobble>> GetCachedAsync();

        Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble);

        Task<ScrobbleResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobbles);

        Task<ScrobbleResponse> SendCachedScrobblesAsync();
    }
}
