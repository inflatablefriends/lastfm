using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Cimbalino.Phone.Toolkit.Services;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Demo.Apollo.Annotations;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace IF.Lastfm.Demo.Apollo.TestPages
{
    public partial class Scrobbling : PhoneApplicationPage
    {
        private ScrobblingTestViewModel _viewModel;

        public Scrobbling()
        {
            _viewModel = new ScrobblingTestViewModel();

            DataContext = _viewModel;

            InitializeComponent();

            MultiApplicationBar.SelectedIndex = 0;
        }

        private void OnDoneClick(object sender, EventArgs e)
        {
            _viewModel.Scrobble().AsAsyncAction();
        }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ScrobblingTestViewModel : BaseViewModel
    {
        private string _track;
        private string _albumArtist;
        private string _artist;
        private string _album;
        private bool _inProgress;
        private bool _successful;

        #region Properties 

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

        public bool Successful
        {
            get { return _successful; }
            set
            {
                if (value.Equals(_successful))
                {
                    return;
                }

                _successful = value;
                OnPropertyChanged();
            }
        }

        public string Artist
        {
            get { return _artist; }
            set
            {
                if (value == _artist)
                {
                    return;
                }

                _artist = value;
                OnPropertyChanged();
            }
        }

        public string Album
        {
            get { return _album; }
            set
            {
                if (value == _album)
                {
                    return;
                }

                _album = value;
                OnPropertyChanged();
            }
        }

        public string Track
        {
            get { return _track; }
            set
            {
                if (value == _track)
                {
                    return;
                }

                _track = value;
                OnPropertyChanged();
            }
        }

        public string AlbumArtist
        {
            get { return _albumArtist; }
            set
            {
                if (value == _albumArtist)
                {
                    return;
                }

                _albumArtist = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public async Task Scrobble()
        {
            InProgress = true;

            var appsettings = new ApplicationSettingsService();

            var apikey = appsettings.Get<string>("apikey");
            var apisecret = appsettings.Get<string>("apisecret");
            var username = appsettings.Get<string>("username");
            var pass = appsettings.Get<string>("pass");

            var auth = new Auth(apikey, apisecret);
            await auth.GetSessionTokenAsync(username, pass);

            var trackApi = new TrackApi(auth);

            var scrobble = new Scrobble(Artist, Album, Track, DateTime.UtcNow, AlbumArtist);

            var response = await trackApi.ScrobbleAsync(scrobble);

            Successful = response.Success;

            InProgress = false;
        }
    }
}