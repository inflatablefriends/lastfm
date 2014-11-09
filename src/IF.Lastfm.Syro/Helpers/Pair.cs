using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IF.Lastfm.Syro.Helpers
{
    public class Pair<TK,TV> : INotifyPropertyChanged
    {
        private TK _key;
        private TV _value;
        
        public TK Key
        {
            get { return _key; }
            set
            {
                if (Equals(_key, value)) return;
                _key = value;
                NotifyPropertyChanged();
            }
        }

        public TV Value
        {
            get { return _value; }
            set
            {
                if (Equals(_value, value)) return;
                _value = value;
                NotifyPropertyChanged();
            }
        }

        public Pair()
        {
        }

        public Pair(TK key, TV value)
            : this()
        {
            Key = key;
            Value = value;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}