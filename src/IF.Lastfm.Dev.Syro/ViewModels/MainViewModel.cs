using IF.Common.Metro.Mvvm;
using IF.Common.Metro.Progress;
using System.Windows.Input;
using Windows.UI.Core;

namespace IF.Lastfm.Dev.Syro.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public ICommand GenerateProgressReportCommand { get; private set; }

        public MainViewModel(CoreDispatcher dispatcher, IProgressAggregator p) : base(dispatcher, p)
        {
            GenerateProgressReportCommand = new AsyncDelegateCommand();
        }
    }
}
