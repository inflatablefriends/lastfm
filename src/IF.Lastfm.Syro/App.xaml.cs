using IF.Lastfm.Syro.ViewModels;
using System.Windows;

namespace IF.Lastfm.Syro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ViewModelLocator _kernel;

        public static ViewModelLocator Kernel { get { return _kernel; } }
    }
}
