using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IF.Lastfm.Syro.Helpers;

namespace IF.Lastfm.Syro.ViewModels
{
    internal class MainState : ViewModelBase
    {
        private Type _selectedLastObjectType;
        private Type _selectedResponseType;
        private Type _selectedCommandType;

        private string _lastPassword;
        private string _lastUsername;

        private string _commandMethodName;
        private string _commandPageNumber;
        private string _commandItemCount;

        private Dictionary<string, string> _commandParameters;

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
        
        public string LastUsername
        {
            get { return _lastUsername; }
            set
            {
                if (value == _lastUsername) return;
                _lastUsername = value;
                OnPropertyChanged();
            }
        }

        public string LastPassword
        {
            get { return _lastPassword; }
            set
            {
                if (value == _lastPassword) return;
                _lastPassword = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, string> CommandParameters
        {
            get { return _commandParameters; }
            set
            {
                if (Equals(value, _commandParameters)) return;
                _commandParameters = value;
                OnPropertyChanged();
            }
        }
    }
}