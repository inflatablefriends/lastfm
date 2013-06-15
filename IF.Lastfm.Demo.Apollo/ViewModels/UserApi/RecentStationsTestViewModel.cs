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
        private Auth _auth;
        private bool _inProgress;
        private PageProgress _stationPageProgress;
        private ObservableCollection<Station> _stations;

        #region Properties 

        public Auth Auth
        {
            get { return _auth; }
            set
            {
                if (Equals(value, _auth))
                {
                    return;
                }

                _auth = value;
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

        public ObservableCollection<Station> Stations
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
            Stations = new ObservableCollection<Station>();
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

            var auth = new Auth(apikey, apisecret);

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

            var response = await userApi.GetRecentStations(_stationPageProgress.ExpectedPage, 5);

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