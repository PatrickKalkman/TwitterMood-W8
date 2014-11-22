using TwitterMood.Management;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TwitterMood
{
    public sealed partial class SettingsContent : UserControl
    {
        public SettingsContent()
        {
            this.InitializeComponent();
            this.Loaded += SettingsContent_Loaded;
        }

        private async void SettingsContent_Loaded(object sender, RoutedEventArgs e)
        {
            TwitterSettingsManager settingsManager = new TwitterSettingsManager(new DebugLogger());
            TwitterMoodSettings settings = await settingsManager.GetSettingsDirect();
            if (settings != null && !string.IsNullOrEmpty(settings.AdditionalSearchTerm))
            {
                AdditionalSearchTerm.Text = settings.AdditionalSearchTerm;
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TwitterSettingsManager settingsManager = new TwitterSettingsManager(new DebugLogger());
            TwitterMoodSettings settings = settingsManager.CreateSettings(AdditionalSearchTerm.Text);
            await settingsManager.SaveSettings(settings);
        }
    }
}
