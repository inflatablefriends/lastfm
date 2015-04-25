using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Scrobblers
{
    public class Scrobbler : ScrobblerBase
    {
        public Scrobbler(ILastAuth auth, HttpClient httpClient = null) : base(auth, httpClient)
        {
            CacheEnabled = false;
        }

        public override Task<IEnumerable<Scrobble>> GetCachedAsync()
        {
            return Task.FromResult(Enumerable.Empty<Scrobble>());
        }

        protected override Task<LastResponseStatus> CacheAsync(IEnumerable<Scrobble> scrobble, LastResponseStatus originalResponseStatus)
        {
            return Task.FromResult(originalResponseStatus);
        }
    }
}