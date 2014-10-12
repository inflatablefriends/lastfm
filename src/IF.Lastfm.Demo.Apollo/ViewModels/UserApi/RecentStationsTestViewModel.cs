using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cimbalino.Phone.Toolkit.Services;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Demo.Apollo.TestPages.ViewModels;

namespace IF.Lastfm.Demo.Apollo.ViewModels.UserApi
{
    public class RecentStationsTestViewModel : BaseViewModel
    {
        private LastAuth _lastAuth;
        private bool _inProgress;
        private PageProgress _stationPageProgress;
        private ObservableCollection<LastStation> _stations;

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

        public ObservableCollection<LastStation> Stations
        {
            get { return _stations; }
            set
            {
                if (Equals(value, _stations)) return;
                _stations = value;
                OnPropertyChanged();
            }
        }

        #endregion
        
        public RecentStationsTestViewModel()
        {
            _stationPageProgress = new PageProgress();
            Stations = new ObservableCollection<LastStation>();
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


        public async Task GetRecentStations()
        {
            if (!_stationPageProgress.CanGoToNextPage())
            {
                return;
            }

            InProgress = true;

            var userApi = new Core.Api.UserApi(Auth);

            var response = await userApi.GetRecentStations(Auth.UserSession.Username, _stationPageProgress.ExpectedPage, 5);

            _stationPageProgress.PageLoaded(response.Success);

            if (response.Success)
            {
                foreach (var s in response.Content)
                {
                    Stations.Add(s);
                }
            }

            _stationPageProgress.TotalPages = response.TotalPages;

            InProgress = false;
        }
    }
}