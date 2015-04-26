using System.Net.Http;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Scrobblers;

namespace IF.Lastfm.Core.Api
{
    public class LastfmClient : ApiBase
    {
        private readonly LastAuth _lastAuth;
        private AlbumApi _albumApi;
        private ArtistApi _artistApi;
        private ChartApi _chartApi;
        private LibraryApi _libraryApi;
        private TrackApi _trackApi;
        private UserApi _userApi;
        private ScrobblerBase _scrobbler;

        public LastAuth Auth
        {
            get { return _lastAuth; }
        }

        public AlbumApi Album
        {
            get { return _albumApi ?? (_albumApi = new AlbumApi(_lastAuth, HttpClient)); }
        }

        public ArtistApi Artist
        {
            get { return _artistApi ?? (_artistApi = new ArtistApi(_lastAuth, HttpClient)); }
        }

        public ChartApi Chart
        {
            get { return _chartApi ?? (_chartApi = new ChartApi(_lastAuth, HttpClient)); }
        }

        public LibraryApi Library
        {
            get { return _libraryApi ?? (_libraryApi = new LibraryApi(_lastAuth, HttpClient)); }
        }

        public ScrobblerBase Scrobbler
        {
            get { return _scrobbler ?? (_scrobbler = new Scrobbler(_lastAuth, HttpClient)); }
        }

        public TrackApi Track
        {
            get { return _trackApi ?? (_trackApi = new TrackApi(_lastAuth, HttpClient)); }
        }

        public UserApi User
        {
            get { return _userApi ?? (_userApi = new UserApi(_lastAuth, HttpClient)); }
        }

        public LastfmClient(string apiKey, string apiSecret, HttpClient httpClient = null, ScrobblerBase scrobbler = null)
            : base(httpClient)
        {
            _lastAuth = new LastAuth(apiKey, apiSecret, httpClient);
            _scrobbler = scrobbler;
        }
        public override void Dispose()
        {
            _lastAuth.Dispose();

            if (_albumApi != null) _albumApi.Dispose();
            if (_artistApi != null) _artistApi.Dispose();
            if (_chartApi != null) _chartApi.Dispose();
            if (_libraryApi != null) _libraryApi.Dispose();
            if (_scrobbler != null) _scrobbler.Dispose();
            if (_trackApi != null) _trackApi.Dispose();
            if (_userApi != null) _userApi.Dispose();

            base.Dispose();
        }
    }
}