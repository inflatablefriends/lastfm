using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using IF.Common.Metro.Mvvm;
using IF.Lastfm.Syro.Tools;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IF.Lastfm.Syro.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private string _solutionDir;
        private bool _generatingProgressReport;
        private List<string> _remainingCommands;
        private int _apiProgress;

        public bool GeneratingProgressReport
        {
            get { return _generatingProgressReport; }
            set
            {
                if (value.Equals(_generatingProgressReport)) return;
                _generatingProgressReport = value;
                OnPropertyChanged();
            }
        }

        public string SolutionDir
        {
            get { return _solutionDir; }
            set
            {
                if (value == _solutionDir) return;
                _solutionDir = value;
                OnPropertyChanged();
            }
        }

        public int ApiProgress
        {
            get { return _apiProgress; }
            set
            {
                if (value.Equals(_apiProgress)) return;
                _apiProgress = value;
                OnPropertyChanged();
            }
        }

        public List<string> RemainingCommands
        {
            get { return _remainingCommands; }
            set
            {
                if (Equals(value, _remainingCommands)) return;
                _remainingCommands = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenerateProgressReportCommand { get; private set; }

        public MainViewModel()
        {
            GenerateProgressReportCommand = new AsyncDelegateCommand(GenerateProgressReport);

            var currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            SolutionDir = Path.GetFullPath(currentDir + "../../../../"); // assuming this is running in debug dir
        }

        private async Task GenerateProgressReport()
        {
            if (GeneratingProgressReport)
            {
                return;
            }

            GeneratingProgressReport = true;

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
                var path = Path.GetFullPath(SolutionDir + "PROGRESS.md");
                ProgressReport.WriteReport(apiGroup, allImplemented, path);

                // ui, duplicating code but w/e
                ApiProgress = (int)ProgressReport.GetPercentage(apiGroup, allImplemented);

                var notimp = new List<string>();
                foreach (var group in apiGroup)
                {
                    var implemented = allImplemented.Where(m => m.StartsWith(group.Key.ToLowerInvariant(), StringComparison.Ordinal)).ToList();
                    var notImplemented = group.Value.Except(implemented).ToList();

                    notimp.AddRange(notImplemented);
                }

                RemainingCommands = notimp;
            });

            GeneratingProgressReport = false;
        }
    }
}
