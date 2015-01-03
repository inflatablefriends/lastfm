using IF.Lastfm.Core.Api.Commands.TrackApi;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api
{
    public class TrackApi : ITrackApi
    {
        public ILastAuth Auth { get; private set; }

        public TrackApi(ILastAuth auth)
        {
            Auth = auth;
        }

        public Task<LastResponse> ScrobbleAsync(Scrobble scrobble)
        {
            var command = new TrackScrobbleCommand(Auth, scrobble);
            return command.ExecuteAsync();
        }

        public async Task<PageResponse<LastShout>> GetShoutsForTrackAsync(string trackname, string artistname, bool autocorrect = false, int page = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTrackShoutsCommand(Auth, trackname, artistname)
                          {
                              Page = page,
                              Count = count,
                              Autocorrect = autocorrect
                          };
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastTrack>> GetInfoAsync(string trackname, string artistname, string username = "")
        {
            var command = new GetTrackInfoCommand(Auth)
                          {
                              TrackName = trackname,
                              ArtistName = artistname,
                              Username = username
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastTrack>> GetInfoByMbidAsync(string mbid)
        {
            var command = new GetTrackInfoCommand(Auth)
            {
                TrackMbid = mbid
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetSimilarTracksAsync(string trackname, string artistname, bool autocorrect = false, int limit = 100)
        {
            var command = new GetSimilarTracksCommand(Auth, trackname, artistname)
            {
                Autocorrect = autocorrect,
                Limit = limit
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> LoveTrackAsync(string trackname, string artistname)
        {
            var command = new LoveTrackCommand(Auth, trackname, artistname);
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> UnloveTrackAsync(string trackname, string artistname)
        {
            var command = new UnloveTrackCommand(Auth, trackname, artistname);
            return await command.ExecuteAsync(); 
        }

        public async Task<PageResponse<LastTrack>> SearchForTrackAsync(string trackname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchTracksCommand(Auth, trackname)
            {
                Page = page,
                Count = itemsPerPage
            };

            return await command.ExecuteAsync();
        }
    }
}