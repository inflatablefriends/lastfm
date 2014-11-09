using System.Windows;
using System.Windows.Controls;
using IF.Lastfm.Syro.ViewModels;

namespace IF.Lastfm.Syro.Pages
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
