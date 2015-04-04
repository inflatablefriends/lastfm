using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.Track;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Scrobblers
{
    public abstract class ScrobblerBase : IScrobbler
    {
        private ILastAuth _auth;

        protected ScrobblerBase(ILastAuth auth)
        {
            _auth = auth;
        }

        public async Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble)
        {
            var cached = await GetCachedAsync();

            var pending = new List<Scrobble>(cached.OrderBy(p => p.TimePlayed))
            {
                scrobble
            };

            var command = new ScrobbleCommand(_auth, pending);

            LastResponse originalResponse = null;
            HttpRequestException exception = null;
            try
            {
                originalResponse = await command.ExecuteAsync();
                if (originalResponse.Success)
                {
                    return new ScrobbleResponse(originalResponse.Status);
                }
            }
            catch (HttpRequestException httpEx)
            {
                exception = httpEx;
            }
            
            ScrobbleResponse cacheResponse;
            try
            {
                await CacheAsync(scrobble);

                cacheResponse = new ScrobbleResponse(LastResponseStatus.Cached);
            }
            catch (Exception e)
            {
                cacheResponse = new ScrobbleResponse(LastResponseStatus.CacheFailed)
                {
                    Exception = e
                };
            }

            return cacheResponse;
        }

        public abstract Task<IEnumerable<Scrobble>> GetCachedAsync();

        public abstract Task CacheAsync(Scrobble scrobble);
    }
}