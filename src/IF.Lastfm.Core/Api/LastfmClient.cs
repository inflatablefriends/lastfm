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
        private ScrobblerBase _scrobbler;
        private TagApi _tagApi;
        private TrackApi _trackApi;
        private UserApi _userApi;

        public LastAuth Auth => _lastAuth;
        public AlbumApi Album => _albumApi ?? (_albumApi = new AlbumApi(_lastAuth, HttpClient));
        public ArtistApi Artist => _artistApi ?? (_artistApi = new ArtistApi(_lastAuth, HttpClient));
        public ChartApi Chart => _chartApi ?? (_chartApi = new ChartApi(_lastAuth, HttpClient));
        public LibraryApi Library => _libraryApi ?? (_libraryApi = new LibraryApi(_lastAuth, HttpClient));
        public TagApi Tag => _tagApi ?? (_tagApi = new TagApi(_lastAuth, HttpClient));
        public TrackApi Track => _trackApi ?? (_trackApi = new TrackApi(_lastAuth, HttpClient));
        public UserApi User => _userApi ?? (_userApi = new UserApi(_lastAuth, HttpClient));

        public ScrobblerBase Scrobbler
        {
            get { return _scrobbler ?? (_scrobbler = new Scrobbler(_lastAuth, HttpClient)); }
            set { _scrobbler = value; }
        }

        public LastfmClient(LastAuth auth, HttpClient httpClient = null, ScrobblerBase scrobbler = null)
            : base(httpClient)
        {
            _lastAuth = auth;
            _scrobbler = scrobbler;
        }

        public LastfmClient(string apiKey, string apiSecret, HttpClient httpClient = null)
            : base(httpClient)
        {
            _lastAuth = new LastAuth(apiKey, apiSecret, httpClient);
        }

        public override void Dispose()
        {
            _lastAuth.Dispose();
            _albumApi?.Dispose();
            _artistApi?.Dispose();
            _chartApi?.Dispose();
            _libraryApi?.Dispose();
            _scrobbler?.Dispose();
            _tagApi?.Dispose();
            _trackApi?.Dispose();
            _userApi?.Dispose();

            base.Dispose();
        }
    }
}