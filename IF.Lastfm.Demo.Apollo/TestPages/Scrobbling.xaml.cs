using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using IF.Lastfm.Demo.Apollo.TestPages.ViewModels;
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

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InProgress")
            {
                if (_viewModel.InProgress)
                {
                    SystemTray.ProgressIndicator = new ProgressIndicator
                    {
                        IsVisible = _viewModel.InProgress,
                        IsIndeterminate = _viewModel.InProgress
                    };
                }
                else
                {
                    SystemTray.ProgressIndicator = null;
                }
            }
        }

        private void OnDoneClick(object sender, EventArgs e)
        {
            if (!_viewModel.InProgress)
            {
                _viewModel.Scrobble().AsAsyncAction();
            }
        }
    }
}