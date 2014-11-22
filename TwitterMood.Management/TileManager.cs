using System;
using TwitterMood.Management.Moods;
using TwitterMood.Task.NotificationsExtensions;
using Windows.UI.Notifications;

namespace TwitterMood.Management
{
    public class TileManager
    {
        private readonly MoodTileManager moodTileManager;

        public TileManager(MoodTileManager moodTileManager)
        {
            this.moodTileManager = moodTileManager;
        }

        public void SendTileUpdate(MoodBase currentMood)
        {
            INotificationContent tileContent = moodTileManager.Create(currentMood);
            var tileNotification = new TileNotification(tileContent.GetXml());
            tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddYears(1);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}