using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cimbalino.Phone.Toolkit.Services;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Demo.Apollo.TestPages.ViewModels;

namespace IF.Lastfm.Demo.Apollo.ViewModels.ArtistApi
{
    public class GetArtistInfoViewModel : BaseViewModel
    {
        private string _artistName;
        private LastArtist _lastArtist;
        private bool _inProgress;
        private IEnumerable<LastTrack> _topTracks;
        private IEnumerable<LastAlbum> _topAlbums;
        private IEnumerable<LastArtist> _similarArtists;

        public string ArtistName
        {
            get { return _artistName; }
            set
            {
                if (value == _artistName)
                {
                    return;
                }

                _artistName = value;
                OnPropertyChanged();
            }
        }

        public LastArtist LastArtist
        {
            get { return _lastArtist; }
            set
            {
                if (Equals(value, _lastArtist))
                {
                    return;
                }

                _lastArtist = value;
                OnPropertyChanged();
            }
        }

        public bool InProgress
        {
            get { return _inProgress; }
            set
            {
                if (value.Equals(_inProgress))
                {
                    return;
                }
                _inProgress = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<LastTrack> TopTracks
        {
            get { return _topTracks; }
            set
            {
                if (Equals(value, _topTracks))
                {
                    return;
                }
                _topTracks = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<LastArtist> SimilarArtists
        {
            get { return _similarArtists; }
            set
            {
                if (Equals(value, _similarArtists)) return;
                _similarArtists = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<LastAlbum> TopAlbums
        {
            get { return _topAlbums; }
            set
            {
                if (Equals(value, _topAlbums))
                {
                    return;
                }
                _topAlbums = value;
                OnPropertyChanged();
            }
        }

        public GetArtistInfoViewModel()
        {
            ArtistName = "Ben Frost";
        }

        public async Task GetInfo()
        {
            InProgress = true;
            
            var appsettings = new ApplicationSettingsService();
            var apikey = appsettings.Get<string>("apikey");
            var apisecret = appsettings.Get<string>("apisecret");
            var username = appsettings.Get<string>("username");
            var pass = appsettings.Get<string>("pass");

            var auth = new LastAuth(apikey, apisecret);

            var response = await auth.GetSessionTokenAsync(username, pass);

            if (response.Success && auth.Authenticated)
            {
                ClearLists();

                var artistApi = new Core.Api.ArtistApi(auth);

                var topTracks = await artistApi.GetTopTracksForArtistAsync(ArtistName);
                if (topTracks.Success)
                {
                    TopTracks = topTracks;
                }

                var topAlbums = await artistApi.GetTopAlbumsForArtistAsync(ArtistName);
                if (topAlbums.Success)
                {
                    TopAlbums = topAlbums;
                }
                
                var similarArtists = await artistApi.GetSimilarArtistsAsync(ArtistName, false, 20);
                if (similarArtists.Success)
                {
                    SimilarArtists = similarArtists;
                }

                var artist = await artistApi.GetArtistInfoAsync(ArtistName);
                if (artist.Success)
                {
                    LastArtist = artist.Content;
                }
            }
            
            InProgress = false;
        }

        private void ClearLists()
        {
            LastArtist = null;
            SimilarArtists = null;
            TopAlbums = null;
            TopTracks = null;
        }
    }
}
