using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using xBrainLab.Security.Cryptography;

namespace IF.Lastfm.Core.Api
{
    public interface ITrackApi
    {
        IAuth Auth { get; }

        Task<LastResponse> ScrobbleAsync(Scrobble scrobble);
        Task<LastResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobble);
    }
}