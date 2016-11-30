using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface ILibraryApi
    {
        ILastAuth Auth { get; }

        Task<PageResponse<LastArtist>> GetArtists(
            string username, 
            DateTimeOffset since,
            int startIndex = 0,
            int endIndex = LastFm.DefaultPageLength);

        Task<PageResponse<LastTrack>> GetTracks(
            string username,
            string artist,
            string album,
            DateTimeOffset since,
            int startIndex = 0,
            int endIndex = LastFm.DefaultPageLength);

        Task<LastResponse> RemoveScrobble(
            string artist,
            string track,
            DateTimeOffset timestamp);

        Task<LastResponse> RemoveTrack(
            string artist,
            string track);
    }
}