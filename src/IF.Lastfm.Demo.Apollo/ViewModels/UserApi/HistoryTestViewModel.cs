using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cimbalino.Phone.Toolkit.Services;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Demo.Apollo.TestPages.ViewModels;

namespace IF.Lastfm.Demo.Apollo.ViewModels.UserApi
{
    public class HistoryTestViewModel : BaseViewModel
    {
        private bool _inProgress;
        private bool _successful;
        private ObservableCollection<LastTrack> _tracks;
        private LastAuth _lastAuth;
        private PageProgress _historyPageProgress;

        #region Properties 

        public LastAuth Auth
        {
            get { return _lastAuth; }
            set
            {
                if (Equals(value, _lastAuth))
                {
                    return;
                }

                _lastAuth = value;
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

        public ObservableCollection<LastTrack> Tracks
        {
            get { return _tracks; }
            set
            {
                if (Equals(value, _tracks))
                {
                    return;
                }

                _tracks = value;
                OnPropertyChanged();
            }
        }

        #endregion
        
        public HistoryTestViewModel()
        {
            _historyPageProgress = new PageProgress();
            Tracks = new ObservableCollection<LastTrack>();
        }

        public async Task NavigatedTo()
        {
            await Authenticate();
        }

        private async Task Authenticate()
        {
            var appsettings = new ApplicationSettingsService();

            var apikey = appsettings.Get<string>("apikey");
            var apisecret = appsettings.Get<string>("apisecret");
            var username = appsettings.Get<string>("username");
            var pass = appsettings.Get<string>("pass");

            var auth = new LastAuth(apikey, apisecret);

            InProgress = true;
            await auth.GetSessionTokenAsync(username, pass);
            InProgress = false;

            Auth = auth;
        }

        public async Task GetHistory()
        {
            if (!_historyPageProgress.CanGoToNextPage())
            {
                return;
            }

            InProgress = true;

            var userApi = new Core.Api.UserApi(Auth);

            var response = await userApi.GetRecentScrobbles(Auth.UserSession.Username, DateTime.UtcNow.AddMonths(-1), _historyPageProgress.ExpectedPage, 50);

            Successful = response.Success;

            _historyPageProgress.PageLoaded(Successful);

            if (response.Success)
            {
                foreach (var track in response.Content)
                {
                    Tracks.Add(track);
                }
            }

            _historyPageProgress.TotalPages = response.TotalPages;

            InProgress = false;
        }
    }
}