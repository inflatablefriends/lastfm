using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.ChartApi;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class ChartApi : IChartApi
    {
        public IAuth Auth { get; private set; }

        public ChartApi(IAuth auth)
        {
            Auth = auth;
        }

        public async Task<PageResponse<LastArtist>> GetTopArtistAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopArtistsCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetTopTracksAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopTracksCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage
            };
            return await command.ExecuteAsync();
        }
    }
}
