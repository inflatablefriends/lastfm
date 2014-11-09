using GalaSoft.MvvmLight.Ioc;
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

            var auth = new LastAuth("a6ab4b9376e54cdb06912bfbd9c1f288", "3aa7202fd1bc6d5a7ac733246cbccc4b");
            SimpleIoc.Default.Register<ILastAuth>(() => auth);
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public T Get<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }
    }
}