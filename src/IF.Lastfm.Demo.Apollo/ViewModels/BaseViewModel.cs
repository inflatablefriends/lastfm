using System.ComponentModel;
using System.Runtime.CompilerServices;
using IF.Lastfm.Demo.Apollo.Annotations;

namespace IF.Lastfm.Demo.Apollo.TestPages.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}