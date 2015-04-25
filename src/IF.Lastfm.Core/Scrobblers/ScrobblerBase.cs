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
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Scrobblers
{
    public abstract class ScrobblerBase : ApiBase, IScrobbler
    {
        private ILastAuth _auth;

        public bool CacheEnabled { get; protected set; }

        internal int MaxBatchSize { get; set; }

        protected ScrobblerBase(ILastAuth auth, HttpClient httpClient = null) : base(httpClient)
        {
            _auth = auth;

            MaxBatchSize = 50;
        }

        public Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble)
        {
            return ScrobbleAsync(new[] {scrobble});
        }

        public async Task<ScrobbleResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobbles)
        {
            var scrobblesList = scrobbles.ToList();
            var cached = await GetCachedAsync();
            var pending = scrobblesList.Concat(cached).OrderBy(s => s.TimePlayed).ToList();
            if (!pending.Any())
            {
                return new ScrobbleResponse(LastResponseStatus.Successful);
            }
            var batches = pending.Batch(MaxBatchSize);
            var responses = new List<LastResponse>(pending.Count / MaxBatchSize);
            var responseExceptions = new List<Exception>();
            foreach(var batch in batches)
            {
                var command = new ScrobbleCommand(_auth, batch)
                {
                    HttpClient = HttpClient
                };

                try
                {
                    var response = await command.ExecuteAsync();

                    responses.Add(response);
                }
                catch (HttpRequestException httpEx)
                {
                    responseExceptions.Add(httpEx);
                }
            }

            if (responses.All(r => r.Success))
            {
                return new ScrobbleResponse(LastResponseStatus.Successful);
            }

            ScrobbleResponse cacheResponse;
            try
            {
                var firstBadResponse = responses.FirstOrDefault(r => !r.Success && r.Status != LastResponseStatus.Unknown);
                var originalResponseStatus = firstBadResponse != null
                    ? firstBadResponse.Status
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