using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Syro.Helpers;
using IF.Lastfm.Syro.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Syro.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private string _solutionDir;
        private bool _generatingProgressReport;
        private List<string> _remainingCommands;
        private int _apiProgress;
        private string _reportPath;
        private ILastAuth _lastAuth;
        private bool _executingCommand;
        private string _commandResult;
        private const string SYRO_CONFIG_FILENAME = "syro.json";
        private const string ReportFilename = "PROGRESS.md";
        private MainState _state;
        private string _configPath;
        private ObservableCollection<Pair<string, string>> _commandParameters;

        #region Binding properties 
        
        public ICommand GenerateProgressReportCommand { get; private set; }
        public ICommand OpenReportCommand { get; private set; }
        public ICommand ExecuteSelectedCommandCommand { get; private set; }
        public ICommand DeleteConfigCommand { get; private set; }

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

        public MainState State
        {
            get { return _state; }
            set
            {
                if (Equals(value, _state)) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Type> BaseCommandTypes { get; private set; }
        
        public IEnumerable<Type> LastObjectTypes { get; private set; }

        public IEnumerable<Type> LastResponseTypes { get; private set; }
        
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

        #endregion

        public MainViewModel(ILastAuth lastAuth)
        {
            _lastAuth = lastAuth;

            GenerateProgressReportCommand = new AsyncDelegateCommand(GenerateProgressReport);
            OpenReportCommand = new DelegateCommand(OpenProgressReport);
            ExecuteSelectedCommandCommand = new AsyncDelegateCommand(ExecuteSelectedCommand);
            DeleteConfigCommand = new DelegateCommand(() =>
            {
                if (_configPath != null && File.Exists(_configPath))
                {
                    File.Delete(_configPath);
                }

                InitialiseState();
            });

            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            SolutionDir = Path.GetFullPath(currentDir + "../../../../"); // assuming this is running in debug dir
            _configPath = Path.GetFullPath(SolutionDir + SYRO_CONFIG_FILENAME);

            BaseCommandTypes = new List<Type>
            {
                typeof(DummyGetAsyncCommand<>),
                typeof(DummyPostAsyncCommand<>)
            };
            LastObjectTypes = Reflektor.FindClassesCastableTo(typeof(ILastfmObject));
            LastResponseTypes = Reflektor.FindClassesCastableTo(typeof(LastResponse));

            InitialiseState();

            Application.Current.Exit += OnAppExit;
        }

        private void InitialiseState()
        {
            var state = LoadState();
            var pairs = state.CommandParameters != null
                ? state.CommandParameters.Select(kv => new Pair<string, string>(kv.Key, kv.Value))
                : Enumerable.Repeat(new Pair<string, string>(), 5);

            State = state;
            CommandParameters = new ObservableCollection<Pair<string, string>>(pairs);
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            var json = JsonConvert.SerializeObject(_state);
            var lines = new[]
            {
                json
            };

            File.WriteAllLines(_configPath, lines);
        }

        public MainState LoadState()
        {
            MainState state = null;
            if (File.Exists(_configPath))
            {
                try
                {
                    var json = File.ReadAllText(_configPath);
                    state = JsonConvert.DeserializeObject<MainState>(json);
                }
                catch
                {
                    state = null;
                }
            }

            if (state == null)
            {
                state = new MainState
                {
                    SelectedBaseCommandType = BaseCommandTypes.FirstOrDefault(),
                    SelectedLastObjectType = LastObjectTypes.FirstOrDefault(),
                    SelectedResponseType = LastResponseTypes.FirstOrDefault(),

                    CommandParameters = new Dictionary<string, string>{
                        {"album", "The Fall of Math"},
                        {"artist", "65daysofstatic"}
                    },
                    CommandMethodName = "album.getInfo",
                    CommandPageNumber = "0",
                    CommandItemCount = "20",
                };
            }

            return state;
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
                var responseType = _state.SelectedResponseType.MakeGenericType(_state.SelectedLastObjectType);
                var genericType = _state.SelectedBaseCommandType.MakeGenericType(responseType);

                if ((_lastAuth.UserSession == null || _lastAuth.UserSession.Username != _state.LastUsername)
                    && _state.SelectedBaseCommandType == typeof(DummyPostAsyncCommand<>))
                {
                    await _lastAuth.GetSessionTokenAsync(_state.LastUsername, _state.LastPassword);
                }

                var instance = Activator.CreateInstance(genericType, _lastAuth);

                var parameters = CommandParameters
                    .Where(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                var methodProperty = genericType.GetProperty("Method", BindingFlags.Public | BindingFlags.Instance);
                methodProperty.SetValue(instance, _state.CommandMethodName);

                if (_state.SelectedResponseType == typeof(PageResponse<>)
                    || _state.CommandMethodName.EndsWith("s")) // yolo
                {
                    var pageProperty = genericType.GetProperty("Page", BindingFlags.Public | BindingFlags.Instance);
                    pageProperty.SetValue(instance, int.Parse(_state.CommandPageNumber));

                    var countProperty = genericType.GetProperty("Count", BindingFlags.Public | BindingFlags.Instance);
                    countProperty.SetValue(instance, int.Parse(_state.CommandItemCount));
                }

                var parametersProperty = genericType.GetProperty("Parameters",
                    BindingFlags.Public | BindingFlags.Instance);
                parametersProperty.SetValue(instance, parameters);
                
                // execute
                var executeMethod = genericType.GetMethods().First(m => m.Name == "ExecuteAsync");
                await (dynamic) executeMethod.Invoke(instance, null);
                
                // cast so we can get the Json response
                var dummyCommand = (IDummyCommand) instance;
                var jo = dummyCommand.Response;

                var formattedJson = jo.ToString(Formatting.Indented);

                // writeout to file
                var filename = string.Format("syro-{0}-{1}.json", _state.CommandMethodName.Replace(".", "-"), DateTime.Now.ToString("yyMMdd-HHmmss"));
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
