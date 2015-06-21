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
        public ILastAuth Auth { get; private set; }

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
            var command = new GetSimilarCommand(Auth, tagName);

            return command.ExecuteAsync();
        }
    }
}