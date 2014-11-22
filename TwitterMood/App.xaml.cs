using System;
using Callisto.Controls;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using TwitterMood.Common.Localytics;
using TwitterMood.Task;
using TwitterMood.ViewModel;
using Win8nl.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TwitterMood
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Localytics tracking.
        /// </summary>
        private LocalyticsSession appSession;

        private readonly ResourceLoader resourceLoader = new ResourceLoader();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        private void SettingCharmManager_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("privacypolicy", "Privacy policy", OpenPrivacyPolicy));

            SettingsCommand cmd = new SettingsCommand("MoodSetting", resourceLoader.GetString("MoodSettings"), (x) =>
            {
                // create a new instance of the flyout
                SettingsFlyout settings = new SettingsFlyout();

                // optionally change header and content background colors away from defaults (recommended)
                settings.ContentBackgroundBrush = new SolidColorBrush(Colors.DarkGray);
                settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
                settings.HeaderText = resourceLoader.GetString("MoodSettings");

                // provide some logo (preferrably the smallogo the app uses)
                var bmp = new BitmapImage(new Uri("ms-appx:///Assets/SmallLogo.png"));
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                settings.Content = new SettingsContent();

                // open it
                settings.IsOpen = true;
            });

            args.Request.ApplicationCommands.Add(cmd);
        }

        private async void OpenPrivacyPolicy(IUICommand command)
        {
            Uri uri = new Uri("http://www.semanticarchitecture.net/TwitterMoodPrivacy.html");
            await Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += SettingCharmManager_CommandsRequested;

            CreateAndUploadLocalyticsSession();

            var rootFrame = Window.Current.Content as Frame;

            SimpleIoc.Default.Register<INavigationService>(() => new NavigationService(rootFrame));

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();

            //var backgroundTaskInstaller = new BackgroundTaskInstaller();
            //backgroundTaskInstaller.Install();

            await AskForReview();


            DispatcherHelper.Initialize();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            CreateAndUploadLocalyticsSession();
            base.OnActivated(args);
        }

        private void CreateAndUploadLocalyticsSession()
        {
            appSession = new LocalyticsSession("c5130e738e3081df2a4a219-eb8a93fe-7b76-11e2-30aa-008e703cf207");
            appSession.Open();
            appSession.Upload();
        }

        private async System.Threading.Tasks.Task AskForReview()
        {
            int started = 0;
            if (Windows.Storage.ApplicationData.Current.RoamingSettings.Values.ContainsKey("started"))
            {
                started = (int)Windows.Storage.ApplicationData.Current.RoamingSettings.Values["started"];
            }

            started++;
            Windows.Storage.ApplicationData.Current.RoamingSettings.Values["started"] = started;

            if (started == 4)
            {
                var md = new MessageDialog(resourceLoader.GetString("ThankYouReview"), resourceLoader.GetString("ThankYouTitle"));
                bool? reviewresult = null;
                md.Commands.Add(new UICommand("OK", (cmd) => reviewresult = true));
                md.Commands.Add(new UICommand("Cancel", (cmd) => reviewresult = false));
                await md.ShowAsync();
                if (reviewresult == true)
                {
                    string familyName = Package.Current.Id.FamilyName;
                    await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", familyName)));
                }
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            appSession.Close();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
