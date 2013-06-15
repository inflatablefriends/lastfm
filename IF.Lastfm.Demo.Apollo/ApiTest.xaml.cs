using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace IF.Lastfm.Demo.Apollo
{
    public partial class ApiTest : PhoneApplicationPage
    {
        public ApiTest()
        {
            InitializeComponent();
        }

        private void OnScrobblingLinkClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/TestPages/Scrobbling.xaml", UriKind.Relative));
        }

        private void OnAppBarSettingsClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnHistoryLinkClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/TestPages/History.xaml", UriKind.Relative));
        }
    }
}