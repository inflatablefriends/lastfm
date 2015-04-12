using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.Track;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Scrobblers
{
    public abstract class ScrobblerBase : ApiBase, IScrobbler
    {
        private ILastAuth _auth;

        public bool CacheEnabled { get; protected set; }

        protected ScrobblerBase(ILastAuth auth, HttpClient httpClient = null) : base(httpClient)
        {
            _auth = auth;
        }

        public Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble)
        {
            return ScrobbleAsync(new[] {scrobble});
        }

        public async Task<ScrobbleResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobbles)
        {
            var cached = await GetCachedAsync();

            var scrobblesList = scrobbles.ToList();
            var pending = cached.Concat(scrobblesList).OrderBy(s => s.TimePlayed);
            if (!pending.Any())
            {
                var response = new ScrobbleResponse(LastResponseStatus.Successful);
                return response;
            }

            var command = new ScrobbleCommand(_auth, pending.FirstOrDefault())
            {
                HttpClient = HttpClient
            };

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
                var originalResponseStatus = originalResponse != null && originalResponse.Status != LastResponseStatus.Unknown
                    ? originalResponse.Status
                    : LastResponseStatus.RequestFailed; // TODO check httpEx

                var cacheStatus = await CacheAsync(scrobblesList, originalResponseStatus);

                cacheResponse = new ScrobbleResponse(cacheStatus);
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

        protected abstract Task<LastResponseStatus> CacheAsync(IEnumerable<Scrobble> scrobble, LastResponseStatus originalResponseStatus);
    }
}