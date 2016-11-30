using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Library;
using IF.Lastfm.Core.Helpers;

namespace IF.Lastfm.Core.Api
{
    public class LibraryApi : ApiBase, ILibraryApi
    {
        public LibraryApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }

        public Task<PageResponse<LastArtist>> GetArtists(string username, DateTimeOffset since, int startIndex = 0, int endIndex = LastFm.DefaultPageLength)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResponse<LastTrack>> GetTracks(string username, string artist, string album, DateTimeOffset since, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTracksCommand(Auth, username, artist, album, since)
            {
                Page = pagenumber,
                Count = count,
                HttpClient = HttpClient
            };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> RemoveScrobble(string artist, string track, DateTimeOffset timestamp)
        {
            var command = new RemoveScrobbleCommand(Auth, artist, track, timestamp)
            {
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<LastResponse> RemoveTrack(string artist, string track)
        {
            var command = new RemoveTrackCommand(Auth, artist, track)
            {
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }
    }
}