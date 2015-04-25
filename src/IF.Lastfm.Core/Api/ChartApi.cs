using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Chart;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class ChartApi : ApiBase, IChartApi
    {
        public ILastAuth Auth { get; private set; }

        public ChartApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }

        public async Task<PageResponse<LastArtist>> GetTopArtistsAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopArtistsCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTrack>> GetTopTracksAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopTracksCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return await command.ExecuteAsync();
        }
    }
}