using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Tag;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class TagApi : ApiBase, ITagApi
    {

        public TagApi(ILastAuth auth, HttpClient httpClient = null)
            : base(httpClient)
        {
            Auth = auth;
        }

        /// <summary>
        /// Search for tags similar to this one. Returns tags ordered by similarity, based on listening data. 
        /// </summary>
        public Task<PageResponse<LastTag>> GetSimilarAsync(string tagName)
        {
            var command = new GetSimilarCommand(Auth, tagName)
            {
                HttpClient = HttpClient
            };

            return command.ExecuteAsync();
        }

        /// <summary>
        /// Get the metadata for a tag.
        /// </summary>
        public Task<LastResponse<LastTag>> GetInfoAsync(string tagName)
        {
            var command = new GetInfoCommand(Auth, tagName)
            {
                HttpClient = HttpClient
            };

            return command.ExecuteAsync();
        }

        /// <summary>
        /// Get the top albums tagged by this tag, ordered by tag count.
        /// </summary>
        public Task<PageResponse<LastAlbum>> GetTopAlbumsAsync(string tagName, int page=1, int itemsPerPage= LastFm.DefaultPageLength)
        {
            var command = new GetTopAlbumsCommand(Auth, tagName)
            {
                HttpClient = HttpClient,
                Page = page,
                Count = itemsPerPage
            };

            return command.ExecuteAsync();
        }

		/// <summary>
		/// Get the top artists tagged by this tag, ordered by tag count.
		/// </summary>
		public Task<PageResponse<LastArtist>> GetTopArtistsAsync(string tagName, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
		{
			var command = new GetTopArtistsCommand(Auth, tagName)
			{
				HttpClient = HttpClient,
				Page = page,
				Count = itemsPerPage
			};

			return command.ExecuteAsync();
		}

		/// <summary>
		/// Get the top artists tagged by this tag, ordered by tag count.
		/// </summary>
		public Task<PageResponse<LastTrack>> GetTopTracksAsync(string tagName, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
		{
			var command = new GetTopTracksCommand(Auth, tagName)
			{
				HttpClient = HttpClient,
				Page = page,
				Count = itemsPerPage
			};

			return command.ExecuteAsync();
		}

		/// <summary>
		/// Fetches the top global tags on Last.fm, sorted by popularity (number of times used).
		/// </summary>
		public Task<PageResponse<LastTag>> GetTopTagsAsync()
        {
            var command = new GetTopTagsCommand(Auth)
            {
                HttpClient = HttpClient
            };

            return command.ExecuteAsync();
        }
    }
}