using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface ILibraryApi
    {
        ILastAuth Auth { get; }

        Task<PageResponse<LastTrack>> GetTracks(string username,
            string artist,
            string album,
            DateTime since,
            int startIndex = 0,
            int endIndex = LastFm.DefaultPageLength);

    }
}