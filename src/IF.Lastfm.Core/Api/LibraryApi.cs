using System;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.LibraryApi;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class LibraryApi : ILibraryApi
    {
        public ILastAuth Auth { get; private set; }

        /// <summary>
        /// Gets scrobbles and stuff
        /// </summary>
        /// <param name="username"></param>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="since"></param>
        /// <param name="pagenumber"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PageResponse<LastTrack>> GetTracks(string username, string artist, string album, DateTime since, int pagenumber = 0, int count = LastFm.DefaultPageLength)
        {
            var command = new GetTracksCommand(Auth, username, artist, album, since)
                          {
                              Page = pagenumber,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }
    }
}