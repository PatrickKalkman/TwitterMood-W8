/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="using:ProjectForTemplates.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TwitterMood.Design;
using TwitterMood.Management;
using TwitterMood.Management.Design;
using TwitterMood.Management.Moods;
using TwitterMood.Management.NotificationsExtensions;
using TwitterMood.Moods;
using TwitterMood.Task;
using TwitterMood.Task.NotificationsExtensions;
using Win8nl.Services;

namespace TwitterMood.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<ILogger, DesignLogger>();
                SimpleIoc.Default.Register<IMoodManager, DesignMoodManager>();
            }
            else
            {
                SimpleIoc.Default.Register<ILogger, DebugLogger>();
                SimpleIoc.Default.Register<IMoodManager, MoodManager>();
            }

            SimpleIoc.Default.Register<TwitterSettingsManager>();
            SimpleIoc.Default.Register<MoodStorageManager, MoodStorageManager>();
            SimpleIoc.Default.Register<BackgroundTaskInstaller>();
            SimpleIoc.Default.Register<CurrentMoodMapper>();
            SimpleIoc.Default.Register<TwitterSearchResultParser>();
            SimpleIoc.Default.Register<TwitterMoodRequest>();
            SimpleIoc.Default.Register<TwitterMoodQueryCreator>();
            SimpleIoc.Default.Register<TwitterResponseStorage>();
            SimpleIoc.Default.Register<TwitterMoodQueryStorage>();
            SimpleIoc.Default.Register<TwitterMoodCalculator>();
            SimpleIoc.Default.Register<MoodToTileContentConverter>();
            SimpleIoc.Default.Register<MoodTileManager>();
            SimpleIoc.Default.Register<TileManager>();
            SimpleIoc.Default.Register<MoodDetailViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MoodDetailViewModel Detail
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MoodDetailViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}