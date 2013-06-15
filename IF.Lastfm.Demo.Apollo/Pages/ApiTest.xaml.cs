using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace IF.Lastfm.Demo.Apollo.Pages
{
    public partial class ApiTest : PhoneApplicationPage
    {
        public ApiTest()
        {
            InitializeComponent();
        }

        private void OnScrobblingLinkClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/TrackApi/Scrobbling.xaml", UriKind.Relative));
        }

        private void OnAppBarSettingsClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnHistoryLinkClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserApi/History.xaml", UriKind.Relative));
        }
    }
}