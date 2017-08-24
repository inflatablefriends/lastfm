using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Scrobblers
{
    public class MemoryScrobbler : ScrobblerBase
    {
        private readonly HashSet<Scrobble> _scrobbles;

        public MemoryScrobbler(ILastAuth auth, HttpClient httpClient = null) : base(auth, httpClient)
        {
            _scrobbles = new HashSet<Scrobble>();
        }

        public override Task<IEnumerable<Scrobble>> GetCachedAsync()
        {
            return Task.FromResult(_scrobbles.AsEnumerable());
        }

        public override Task RemoveFromCacheAsync(ICollection<Scrobble> scrobbles)
        {
            foreach (var scrobble in scrobbles)
            {
                _scrobbles.Remove(scrobble);
            }

            return Task.FromResult(0);
        }

        public override Task<int> GetCachedCountAsync()
        {
            return Task.FromResult(_scrobbles.Count);
        }

        protected override Task<LastResponseStatus> CacheAsync(IEnumerable<Scrobble> scrobbles, LastResponseStatus reason)
        {
            foreach (var scrobble in scrobbles)
            {
                _scrobbles.Add(scrobble);
            }
            return Task.FromResult(LastResponseStatus.Cached);
        }
    }
}