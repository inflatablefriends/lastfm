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

        public ChartApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }

        /// <summary>
        /// Get a list of the most-scrobbled artists on Last.fm.
        /// </summary>
        /// <remarks>
        /// Bug 28/05/16 - itemsPerPage parameter doesn't seem to work all the time; certain values cause more or fewer items to be returned
        /// </remarks>
        public Task<PageResponse<LastArtist>> GetTopArtistsAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopArtistsCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return command.ExecuteAsync();
        }

        /// <summary>
        /// Get a list of the most-scrobbled tracks on Last.fm.
        /// </summary>
        /// <remarks>
        /// Bug 28/05/16 - itemsPerPage parameter doesn't seem to work all the time; certain values cause more or fewer items to be returned
        /// </remarks>
        public Task<PageResponse<LastTrack>> GetTopTracksAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopTracksCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return command.ExecuteAsync();
        }
        
        /// <summary>
        /// Get a list of the most frequently used tags by Last.fm users
        /// </summary>
        /// <remarks>
        /// Bug 28/05/16 - page and itemsPerPage parameters do not actually affect the number of or selection of tags returned
        /// </remarks>
        public Task<PageResponse<LastTag>> GetTopTagsAsync(int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new GetTopTagsCommand(Auth)
            {
                Page = page,
                Count = itemsPerPage,
                HttpClient = HttpClient
            };
            return command.ExecuteAsync();
        }
    }
}