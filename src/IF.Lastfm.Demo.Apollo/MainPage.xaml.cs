using System;
using Cimbalino.Phone.Toolkit.Services;
using Microsoft.Phone.Controls;

namespace IF.Lastfm.Demo.Apollo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            MultiApplicationBar.SelectedIndex = 0;

            var service = new ApplicationSettingsService();

            if (string.IsNullOrEmpty(service.Get<string>("apikey")))
            {
                ApiKey.Text = "a6ab4b9376e54cdb06912bfbd9c1f288";
                ApiSecret.Text = "3aa7202fd1bc6d5a7ac733246cbccc4b";
            }
            else
            {
                ApiKey.Text = service.Get<string>("apikey");
                ApiSecret.Text = service.Get<string>("apisecret");
                Username.Text = service.Get<string>("username");
                Password.Text = service.Get<string>("pass");
            }
        }

        private void OnDoneClick(object sender, EventArgs e)
        {
            var service = new ApplicationSettingsService();

            service.Set("apikey", ApiKey.Text);
            service.Set("apisecret", ApiSecret.Text);
            service.Set("username", Username.Text);
            service.Set("pass", Password.Text);

            service.Save();

            NavigationService.Navigate(new Uri("/Pages/ApiTest.xaml", UriKind.Relative));
        }
    }
}