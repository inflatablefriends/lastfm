using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Syro.Helpers;

namespace IF.Lastfm.Syro
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            RootFrame.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));

            var timestampCommand = new DelegateCommand(OnTimestampKeyBinding);
            var timestampGesture = new KeyGesture(Key.T, ModifierKeys.Alt);
            var timestampBinding = new KeyBinding(timestampCommand, timestampGesture);
            InputBindings.Add(timestampBinding);
        }

        private void OnTimestampKeyBinding()
        {
            var window = this;
            var focusedTextbox = FocusManager.GetFocusedElement(window) as TextBox;

            if (focusedTextbox != null)
            {
                var currentTimestamp = DateTimeOffset.UtcNow.AsUnixTime().ToString();
                focusedTextbox.Text = focusedTextbox.Text.Insert(focusedTextbox.CaretIndex, currentTimestamp);
            }
        }
    }
}
