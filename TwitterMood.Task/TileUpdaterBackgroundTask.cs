using GalaSoft.MvvmLight.Messaging;
using TwitterMood.Management;
using TwitterMood.Management.Moods;
using TwitterMood.Management.NotificationsExtensions;
using TwitterMood.Task.NotificationsExtensions;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace TwitterMood.Task
{
    public sealed class TileUpdaterBackgroundTask : IBackgroundTask
    {
        private readonly MoodManager moodManager;
        private readonly MoodTileManager moodTileManager;
        private BackgroundTaskDeferral deferral;

        public TileUpdaterBackgroundTask()
        {
            var logger = new DebugLogger();

            this.moodManager = new MoodManager(
                new TwitterMoodQueryCreator(), 
                new TwitterMoodQueryStorage(), 
                new TwitterMoodRequest(),
                new TwitterSearchResultParser(logger),
                new TwitterMoodCalculator(),
                new TwitterSettingsManager(logger),
                new TwitterResponseStorage(logger), 
                new DebugLogger());

            this.moodManager.Initialize();
            this.moodTileManager = new MoodTileManager(new MoodToTileContentConverter());
            Messenger.Default.Register<MoodsRetrieved>(this, OnMoodsRetrieved);
        }
        
        private void OnMoodsRetrieved(MoodsRetrieved moods)
        {
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.EnableNotificationQueue(true);
            MoodBase dominantMood = moodManager.GetDominantMood();
            INotificationContent tile = moodTileManager.Create(dominantMood);
            tileUpdater.Update(new TileNotification(tile.GetXml()));
            deferral.Complete();
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.EnableNotificationQueue(true);
            moodManager.Execute();
        }
    }
}
