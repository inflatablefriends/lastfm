using System;

namespace IF.Lastfm.Syro
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            RootFrame.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}
