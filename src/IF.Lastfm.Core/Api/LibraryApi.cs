using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Library;

namespace IF.Lastfm.Core.Api
{
    public class LibraryApi : ILibraryApi
    {
        public LibraryApi( ILastAuth auth ) { Auth = auth; }
        public ILastAuth Auth { get; private set; }


        public async Task<PageResponse<LastTrack>> GetTracks(string username, string artist, string album, DateTimeOffset since, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTracksCommand(Auth, username, artist, album, since)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> RemoveScrobble( string artist, string track, DateTimeOffset timestamp ) {
            var command = new RemoveScrobbleCommand( Auth, artist, track, timestamp );
            return await command.ExecuteAsync();
        }
        public async Task<LastResponse> RemoveTrack( string artist, string track ) {
            var command = new RemoveTrackCommand( Auth, artist, track );
            return await command.ExecuteAsync();
        }
    }
}