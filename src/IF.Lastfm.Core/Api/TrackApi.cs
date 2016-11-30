using System;
using System.Net.Http;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Track;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Scrobblers;

namespace IF.Lastfm.Core.Api
{
    public class TrackApi : ApiBase, ITrackApi
    {

        public TrackApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }

        [Obsolete("This method has been moved to the Scrobbler class. More information can be found at " + LastFm.SCROBBLING_HELP_URL)]
        public Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble)
        {
            var command = new ScrobbleCommand(Auth, scrobble)
            {
                HttpClient = HttpClient
            };
            return command.ExecuteAsync();
        }

        public Task<LastResponse> UpdateNowPlayingAsync(Scrobble scrobble)
        {
            var command = new UpdateNowPlayingCommand(Auth, scrobble)
            {
                HttpClient = HttpClient
            };
            return command.ExecuteAsync();
        }

        public async Task<PageResponse<LastShout>> GetShoutsForTrackAsync(string trackname, string artistname, bool autocorrect = false, int page = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetShoutsCommand(Auth, trackname, artistname)
            {
                Page = page,
                Count = count,
                Autocorrect = autocorrect,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastTrack>> GetInfoAsync(string trackname, string artistname, string username = "")
        {
            var command = new GetInfoCommand(Auth)
            {
                TrackName = trackname,
                ArtistName = artistname,
                Username = username,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastTrack>> GetInfoByMbidAsync(string mbid)
        {
            var command = new GetInfoCommand(Auth)
            {
                TrackMbid = mbid,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetSimilarAsync(string trackname, string artistname, bool autocorrect = false, int limit = 100)
        {
            var command = new GetSimilarCommand(Auth, trackname, artistname)
            {
                Autocorrect = autocorrect,
                Limit = limit,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> LoveAsync(string trackname, string artistname)
        {
            var command = new LoveCommand(Auth, trackname, artistname)
            {
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> UnloveAsync(string trackname, string artistname)
        {
            var command = new UnloveCommand(Auth, trackname, artistname)
            {
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync(); 
        }

        public async Task<PageResponse<LastTrack>> SearchAsync(string trackname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchCommand(Auth, trackname)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }
    }
}