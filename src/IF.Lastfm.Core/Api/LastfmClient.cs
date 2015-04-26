using System.Net.Http;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Scrobblers;

namespace IF.Lastfm.Core.Api
{
    public class LastfmClient : ApiBase
    {
        private readonly ILastAuth _lastAuth;
        private IAlbumApi _albumApi;
        private IArtistApi _artistApi;
        private IChartApi _chartApi;
        private ILibraryApi _libraryApi;
        private ITrackApi _trackApi;
        private IUserApi _userApi;
        private IScrobbler _scrobbler;

        public ILastAuth Auth
        {
            get { return _lastAuth; }
        }

        public IAlbumApi Album
        {
            get { return _albumApi ?? (_albumApi = new AlbumApi(_lastAuth, HttpClient)); }
        }

        public IArtistApi Artist
        {
            get { return _artistApi ?? (_artistApi = new ArtistApi(_lastAuth, HttpClient)); }
        }

        public IChartApi Chart
        {
            get { return _chartApi ?? (_chartApi = new ChartApi(_lastAuth, HttpClient)); }
        }

        public ILibraryApi Library
        {
            get { return _libraryApi ?? (_libraryApi = new LibraryApi(_lastAuth, HttpClient)); }
        }

        public IScrobbler Scrobbler
        {
            get { return _scrobbler ?? (_scrobbler = new Scrobbler(_lastAuth, HttpClient)); }
        }

        public ITrackApi Track
        {
            get { return _trackApi ?? (_trackApi = new TrackApi(_lastAuth, HttpClient)); }
        }

        public IUserApi User
        {
            get { return _userApi ?? (_userApi = new UserApi(_lastAuth, HttpClient)); }
        }

        public LastfmClient(string apiKey, string apiSecret, HttpClient httpClient = null, IScrobbler scrobbler = null)
            : base(httpClient)
        {
            _lastAuth = new LastAuth(apiKey, apiSecret, httpClient);
            _scrobbler = scrobbler;
        }
    }
}