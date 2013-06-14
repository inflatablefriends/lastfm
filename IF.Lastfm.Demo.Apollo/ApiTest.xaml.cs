using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
    }
}