using GalaSoft.MvvmLight.Ioc;
using IF.Lastfm.Core;
using IF.Lastfm.Core.Api;
using Microsoft.Practices.ServiceLocation;

namespace IF.Lastfm.Syro.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var auth = new LastAuth(LastFm.TEST_APIKEY, LastFm.TEST_APISECRET);
            SimpleIoc.Default.Register<ILastAuth>(() => auth);
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public T Get<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }
    }
}