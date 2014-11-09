using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using IF.Lastfm.Core;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands;
using IF.Lastfm.Core.Api.Commands.UserApi;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Syro.Helpers;
using IF.Lastfm.Syro.Tools;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;

namespace IF.Lastfm.Syro.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private string _solutionDir;
        private bool _generatingProgressReport;
        private List<string> _remainingCommands;
        private int _apiProgress;
        private string _reportPath;
        private Type _selectedCommandType;
        private ObservableCollection<Pair<string, string>> _commandParameters;
        private Type _selectedLastObjectType;
        private bool _executingCommand;
        private Type _selectedResponseType;
        private ILastAuth _lastAuth;
        private string _commandResult;
        private string _commandMethodName;
        private string _commandPageNumber;
        private string _commandItemCount;
        private const string ReportFilename = "PROGRESS.md";

        #region Binding properties 
        
        public ICommand GenerateProgressReportCommand { get; private set; }
        public ICommand OpenReportCommand { get; private set; }
        public ICommand ExecuteSelectedCommandCommand { get; private set; }

        public string CommandResult
        {
            get { return _commandResult; }
            set
            {
                if (value == _commandResult) return;
                _commandResult = value;
                OnPropertyChanged();
            }
        }

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

        public IEnumerable<Type> BaseCommandTypes { get; private set; }

        public Type SelectedBaseCommandType
        {
            get { return _selectedCommandType; }
            set
            {
                if (value == _selectedCommandType) return;

                _selectedCommandType = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Type> LastObjectTypes { get; private set; }

        public Type SelectedLastObjectType
        {
            get { return _selectedLastObjectType; }
            set
            {
                if (value == _selectedLastObjectType) return;
                _selectedLastObjectType = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Type> LastResponseTypes { get; private set; }

        public Type SelectedResponseType
        {
            get { return _selectedResponseType; }
            set
            {
                if (value == _selectedResponseType) return;
                _selectedResponseType = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Pair<string, string>> CommandParameters
        {
            get { return _commandParameters; }
            set
            {
                if (Equals(value, _commandParameters)) return;
                _commandParameters = value;
                OnPropertyChanged();
            }
        }

        public bool ExecutingCommand
        {
            get { return _executingCommand; }
            set
            {
                if (value.Equals(_executingCommand)) return;
                _executingCommand = value;
                OnPropertyChanged();
            }
        }

        public string CommandMethodName
        {
            get { return _commandMethodName; }
            set
            {
                if (value == _commandMethodName) return;
                _commandMethodName = value;
                OnPropertyChanged();
            }
        }

        public string CommandPageNumber
        {
            get { return _commandPageNumber; }
            set
            {
                if (value == _commandPageNumber) return;
                _commandPageNumber = value;
                OnPropertyChanged();
            }
        }

        public string CommandItemCount
        {
            get { return _commandItemCount; }
            set
            {
                if (value == _commandItemCount) return;
                _commandItemCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainViewModel(ILastAuth lastAuth)
        {
            _lastAuth = lastAuth;

            GenerateProgressReportCommand = new AsyncDelegateCommand(GenerateProgressReport);
            OpenReportCommand = new DelegateCommand(OpenProgressReport);
            ExecuteSelectedCommandCommand = new AsyncDelegateCommand(ExecuteSelectedCommand);

            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            SolutionDir = Path.GetFullPath(currentDir + "../../../../"); // assuming this is running in debug dir

            BaseCommandTypes = new List<Type>
            {
                typeof(DummyGetAsyncCommand<>),
                typeof(DummyPostAsyncCommand<>)
            };
            LastObjectTypes = Reflektor.FindClassesCastableTo(typeof (ILastfmObject));
            LastResponseTypes = Reflektor.FindClassesCastableTo(typeof (LastResponse));

            SelectedBaseCommandType = BaseCommandTypes.FirstOrDefault();
            SelectedLastObjectType = LastObjectTypes.FirstOrDefault();
            SelectedResponseType = LastResponseTypes.FirstOrDefault();

            CommandParameters = new ObservableCollection<Pair<string, string>>(new List<Pair<string, string>>
            {
                new Pair<string, string>(),
                new Pair<string, string>(),
                new Pair<string, string>(),
                new Pair<string, string>(),
                new Pair<string, string>()
            });

            CommandMethodName = "album.getInfo";
            CommandPageNumber = "0";
            CommandItemCount = "20";
        }

        private async Task ExecuteSelectedCommand()
        {
            if (ExecutingCommand)
            {
                return;
            }

            ExecutingCommand = true;
            
            try
            {
                // build up the command<response<lastobject>>
                var responseType = SelectedResponseType.MakeGenericType(SelectedLastObjectType);
                var genericType = SelectedBaseCommandType.MakeGenericType(responseType);

                if (!_lastAuth.Authenticated)
                {
                    await _lastAuth.GetSessionTokenAsync("tehrikkit", "#facedusk17a");
                }

                var instance = Activator.CreateInstance(genericType, _lastAuth);

                var parameters = CommandParameters
                    .Where(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                var methodProperty = genericType.GetProperty("Method", BindingFlags.Public | BindingFlags.Instance);
                methodProperty.SetValue(instance, CommandMethodName);

                if (SelectedResponseType == typeof(PageResponse<>))
                {
                    var pageProperty = genericType.GetProperty("Page", BindingFlags.Public | BindingFlags.Instance);
                    pageProperty.SetValue(instance, int.Parse(CommandPageNumber));

                    var countProperty = genericType.GetProperty("Count", BindingFlags.Public | BindingFlags.Instance);
                    countProperty.SetValue(instance, int.Parse(CommandItemCount));

                    var addPageParamsMethod = genericType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.Name == "AddPagingParameters");
                    addPageParamsMethod.Invoke(instance, null);
                }

                var parametersProperty = genericType.GetProperty("Parameters",
                    BindingFlags.Public | BindingFlags.Instance);
                parametersProperty.SetValue(instance, parameters);

                //test
                var command = new GetRecentStationsCommand(_lastAuth, "tehrikkit")
                {
                    Count = 5,
                    Page = 1
                };
                await command.ExecuteAsync();

                // execute
                var executeMethod = genericType.GetMethods().First(m => m.Name == "ExecuteAsync");
                await (dynamic) executeMethod.Invoke(instance, null);

                

                // cast so we can get the Json response
                var dummyCommand = (IDummyCommand) instance;
                var jo = dummyCommand.Response;

                var formattedJson = jo.ToString(Formatting.Indented);

                // writeout to file
                var filename = string.Format("syro-{0}-{1}.json", jo.Properties().First().Name,
                    DateTime.Now.ToString("yyMMdd-HHmmss"));
                var tempDirPath = Path.GetFullPath(SolutionDir + "tmp/");

                if (!Directory.Exists(tempDirPath))
                {
                    Directory.CreateDirectory(tempDirPath);
                }
                var path = Path.GetFullPath(tempDirPath + filename);

                // write to output directory and launch
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(formattedJson);
                    }
                }
                Process.Start(path);

                CommandResult = formattedJson;
            }
            finally
            {
                ExecutingCommand = false;
            }
        }

        #region Progress report

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
                var allImplemented = Reflektor.GetImplementedCommands().Select(Reflektor.CreateCommand).Select(c => c.Method).ToList();

                // generate the markdown
                _reportPath = Path.GetFullPath(SolutionDir + ReportFilename);
                ProgressReport.WriteReport(apiGroup, allImplemented, _reportPath);

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

        private void OpenProgressReport()
        {
            var path = _reportPath ?? Path.GetFullPath(SolutionDir + ReportFilename);

            Process.Start(path);
        }

        #endregion
    }
}
