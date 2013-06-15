using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using IF.Lastfm.Demo.Apollo.TestPages.ViewModels;
using IF.Lastfm.Demo.Apollo.ViewModels.UserApi;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace IF.Lastfm.Demo.Apollo.Pages.UserApi
{
    public partial class History : PhoneApplicationPage
    {
        private HistoryTestViewModel _viewModel;

        public History()
        {
            _viewModel = new HistoryTestViewModel();

            DataContext = _viewModel;

            InitializeComponent();

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.NavigatedTo();
            await _viewModel.GetHistory();

            var element = VisualTreeHelper.GetChild(PageScroller, 0) as FrameworkElement;
            if (element != null)
            {
                //var group = FindVisualState(element, "ScrollStates");
                //if (group != null)
                //{
                //    group.CurrentStateChanging += group_CurrentStateChanging;
                //}


                var vgroup = FindVisualState(element, "VerticalCompression");
                if (vgroup != null)
                {
                    vgroup.CurrentStateChanging += OnScrolled;
                }

                //var hgroup = FindVisualState(element, "HorizontalCompression");
                //if (hgroup != null)
                //{
                //    hgroup.CurrentStateChanging += hgroup_CurrentStateChanging;
                //}
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InProgress")
            {
                if (_viewModel.InProgress)
                {
                    SystemTray.ProgressIndicator = new ProgressIndicator
                                                       {
                                                           IsVisible = _viewModel.InProgress,
                                                           IsIndeterminate = _viewModel.InProgress
                                                       };
                }
                else
                {
                    SystemTray.ProgressIndicator = null;
                }
            }
        }

        private async void OnScrolled(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            if (PageScroller.VerticalOffset >= PageScroller.ScrollableHeight - 10)
            {
                if (!_viewModel.InProgress)
                {
                    await _viewModel.GetHistory();
                }
            }
        }

        private UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                    {
                        return element as UIElement;
                    }
                    else
                    {
                        returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement,
                                                             targetType);
                    }
                }
            }
            return returnElement;
        }


        private VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            IList groups = VisualStateManager.GetVisualStateGroups(element);
            foreach (VisualStateGroup group in groups)
                if (group.Name == name)
                    return group;

            return null;
        }
    }
}