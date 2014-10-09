using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IChartApi
    {
        IAuth Auth { get; }

        Task<PageResponse<LastArtist>> GetTopArtistsAsync(
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);

        Task<PageResponse<LastTrack>> GetTopTracksAsync(
            int page = 1,
            int itemsPerPage = LastFm.DefaultPageLength);
    }
}
