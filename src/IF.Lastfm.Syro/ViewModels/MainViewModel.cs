using IF.Common.Metro.Mvvm;
using IF.Lastfm.Syro.Tools;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IF.Lastfm.Syro.ViewModels
{
    internal class MainViewModel
    {
        public ICommand GenerateProgressReportCommand { get; private set; }

        public MainViewModel()
        {
            GenerateProgressReportCommand = new AsyncDelegateCommand(GenerateProgressReport);
        }

        private async Task GenerateProgressReport()
        {
            await Task.Run(() =>
            {
                // scrape Last.fm API documentation
                var apiGroup = ProgressReport.GetApiMethods();
                if (apiGroup == null)
                {
                    return;
                }

                // reflect on Last.fm assembly to find all implemented commands
                var allImplemented = ProgressReport.GetImplementedCommands().ToList();

                // generate the markdown
                ProgressReport.WriteReport(apiGroup, allImplemented);

                // file is copied to the solution root by a post-build script.
                // TODO configure build dir in syro
            });
        }
    }
}
