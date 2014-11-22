using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using WinRtUtility;

namespace TwitterMood.Management
{
    public class TwitterSettingsManager
    {
        private readonly ILogger logger;

        public TwitterSettingsManager(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<TwitterMoodSettings> GetSettingsDirect()
        {
            var objectStorageHelper = new ObjectStorageHelper<TwitterMoodSettings>(StorageType.Local);
            TwitterMoodSettings settings = await objectStorageHelper.LoadAsync() ?? new TwitterMoodSettings();
            return settings;
        }

        public async Task<bool> SaveSettings(TwitterMoodSettings settings)
        {
            var objectStorageHelper = new ObjectStorageHelper<TwitterMoodSettings>(StorageType.Local);
            await objectStorageHelper.SaveAsync(settings);
            Messenger.Default.Send(new SettingsSavedEvent {Settings = settings});
            return true;
        }

        public TwitterMoodSettings CreateSettings(string additionalSearchTerm)
        {
            return new TwitterMoodSettings { AdditionalSearchTerm = additionalSearchTerm };
        }
    }
}