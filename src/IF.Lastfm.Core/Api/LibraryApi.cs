using IF.Lastfm.Core.Api.Commands.LibraryApi;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using System;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api
{
    public class LibraryApi : ILibraryApi
    {
        public ILastAuth Auth { get; private set; }

        public async Task<PageResponse<LastTrack>> GetTracks(string username, string artist, string album, DateTime since, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new LibraryGetTracksCommand(Auth, username, artist, album, since)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }
    }
}