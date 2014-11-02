using IF.Lastfm.Dev.Syro.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IF.Lastfm.Dev.Syro.Pages
{
    public sealed partial class MainPage : Page
    {
        private readonly MainViewModel _vm;

        public MainPage()
        {
            _vm = App.Kernel.Get<MainViewModel>();

            DataContext = _vm;

            InitializeComponent();
        }
    }
}
