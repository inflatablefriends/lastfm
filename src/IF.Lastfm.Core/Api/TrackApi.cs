using System;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Track;

namespace IF.Lastfm.Core.Api
{
    public class TrackApi : ITrackApi
    {
        public ILastAuth Auth { get; private set; }

        public TrackApi(ILastAuth auth)
        {
            Auth = auth;
        }

        [Obsolete("This method has been moved to the Scrobbler class. More information can be found at " + LastFm.SCROBBLING_HELP_URL)]
        public Task<LastResponse> ScrobbleAsync(Scrobble scrobble)
        {
            var command = new ScrobbleCommand(Auth, scrobble);
            return command.ExecuteAsync();
        }

        public Task<LastResponse> UpdateNowPlayingAsync(Scrobble scrobble)
        {
            var command = new UpdateNowPlayingCommand(Auth, scrobble);
            return command.ExecuteAsync();
        }

        public async Task<PageResponse<LastShout>> GetShoutsForTrackAsync(string trackname, string artistname, bool autocorrect = false, int page = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetShoutsCommand(Auth, trackname, artistname)
                          {
                              Page = page,
                              Count = count,
                              Autocorrect = autocorrect
                          };
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastTrack>> GetInfoAsync(string trackname, string artistname, string username = "")
        {
            var command = new GetInfoCommand(Auth)
                          {
                              TrackName = trackname,
                              ArtistName = artistname,
                              Username = username
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastTrack>> GetInfoByMbidAsync(string mbid)
        {
            var command = new GetInfoCommand(Auth)
            {
                TrackMbid = mbid
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetSimilarTracksAsync(string trackname, string artistname, bool autocorrect = false, int limit = 100)
        {
            var command = new GetSimilarCommand(Auth, trackname, artistname)
            {
                Autocorrect = autocorrect,
                Limit = limit
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> LoveTrackAsync(string trackname, string artistname)
        {
            var command = new LoveCommand(Auth, trackname, artistname);
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> UnloveTrackAsync(string trackname, string artistname)
        {
            var command = new UnloveCommand(Auth, trackname, artistname);
            return await command.ExecuteAsync(); 
        }

        public async Task<PageResponse<LastTrack>> SearchForTrackAsync(string trackname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchCommand(Auth, trackname)
            {
                Page = page,
                Count = itemsPerPage
            };

            return await command.ExecuteAsync();
        }
    }
}