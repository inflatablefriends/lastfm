using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.Track;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Scrobblers
{
    public abstract class ScrobblerBase : ApiBase, IScrobbler
    {
        public event EventHandler<ScrobbleResponse> AfterSend;

        internal int MaxBatchSize { get; set; }

        protected ScrobblerBase(ILastAuth auth, HttpClient httpClient = null) : base(httpClient)
        {
            Auth = auth;

            MaxBatchSize = 50;
        }
        
        public Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble)
        {
            return ScrobbleAsync(new[] {scrobble});
        }

        public Task<ScrobbleResponse> ScrobbleAsync(IEnumerable<Scrobble> scrobbles)
        {
            return ScrobbleAsyncInternal(scrobbles);
        }

        public Task<ScrobbleResponse> SendCachedScrobblesAsync()
        {
            return ScrobbleAsyncInternal(Enumerable.Empty<Scrobble>());
        }

        public async Task<ScrobbleResponse> ScrobbleAsyncInternal(IEnumerable<Scrobble> scrobbles)
        {
            var scrobblesList = new ReadOnlyCollection<Scrobble>(scrobbles.ToList());
            var cached = await GetCachedAsync();
            var pending = scrobblesList.Concat(cached).OrderBy(s => s.TimePlayed).ToList();
            if (!pending.Any())
            {
                return new ScrobbleResponse(LastResponseStatus.Successful);
            }

            var batches = pending.Batch(MaxBatchSize);
            var responses = new List<ScrobbleResponse>(pending.Count % MaxBatchSize);
            var responseExceptions = new List<Exception>();
            foreach(var batch in batches)
            {
                var command = new ScrobbleCommand(Auth, batch)
                {
                    HttpClient = HttpClient
                };

                try
                {
                    var response = await command.ExecuteAsync();
                    
                    var acceptedMap = new HashSet<Scrobble>(scrobblesList);
                    foreach (var ignored in response.Ignored)
                    {
                        acceptedMap.Remove(ignored);
                    }
                    
                    await RemoveFromCacheAsync(acceptedMap);
                    
                    responses.Add(response);
                }
                catch (HttpRequestException httpEx)
                {
                    responseExceptions.Add(httpEx);
                }
            }

            ScrobbleResponse scrobblerResponse;
            if (!responses.Any() || responses.All(r => r.Success))
            {
                scrobblerResponse = new ScrobbleResponse(LastResponseStatus.Successful);
            }
            else
            {
                var firstBadResponse = responses.FirstOrDefault(r => !r.Success && r.Status != LastResponseStatus.Unknown);
                var originalResponseStatus = firstBadResponse?.Status ?? LastResponseStatus.RequestFailed; // TODO check httpEx

                var cacheStatus = await CacheAsync(scrobblesList, originalResponseStatus);

                scrobblerResponse = new ScrobbleResponse(cacheStatus);
            }
            
            scrobblerResponse.Ignored = responses.SelectMany(r => r.Ignored);

            AfterSend?.Invoke(this, scrobblerResponse);

            return scrobblerResponse;
        }

        public abstract Task<IEnumerable<Scrobble>> GetCachedAsync();

        public abstract Task RemoveFromCacheAsync(ICollection<Scrobble> scrobbles);

        public abstract Task<int> GetCachedCountAsync();

        protected abstract Task<LastResponseStatus> CacheAsync(IEnumerable<Scrobble> scrobble, LastResponseStatus reason);
    }
}