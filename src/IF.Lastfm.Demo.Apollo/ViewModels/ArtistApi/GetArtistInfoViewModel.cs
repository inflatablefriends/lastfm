﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        private FmArtist _fmArtist;
        private bool _inProgress;

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

        public FmArtist FmArtist
        {
            get { return _fmArtist; }
            set
            {
                if (Equals(value, _fmArtist))
                {
                    return;
                }

                _fmArtist = value;
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

        public async Task GetInfo()
        {
            InProgress = true;


            var appsettings = new ApplicationSettingsService();
            var apikey = appsettings.Get<string>("apikey");
            var apisecret = appsettings.Get<string>("apisecret");
            var username = appsettings.Get<string>("username");
            var pass = appsettings.Get<string>("pass");

            var auth = new Auth(apikey, apisecret);

            var response = await auth.GetSessionTokenAsync(username, pass);

            if (response.Success && auth.HasAuthenticated)
            {
                var artistApi = new Core.Api.ArtistApi(auth);

                var artist = await artistApi.GetArtistInfoAsync(ArtistName);
                if (artist.Success)
                {
                    FmArtist = artist.Content;
                }
            }
            
            InProgress = false;
        }
    }
}
