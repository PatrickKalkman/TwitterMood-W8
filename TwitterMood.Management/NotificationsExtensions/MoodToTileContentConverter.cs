using System;
using TwitterMood.Management.Moods;
using TwitterMood.Task.NotificationsExtensions;
using Windows.ApplicationModel.Resources;

namespace TwitterMood.Management.NotificationsExtensions
{
    public sealed class MoodToTileContentConverter
    {
        private readonly ResourceLoader loader = new ResourceLoader();
        
        public MoodTileContent GetTileContent(MoodBase currentMood)
        {
            var moodTileContent = new MoodTileContent();
            moodTileContent.MoodTitle = String.Format(loader.GetString("MoodTileTitle"), currentMood.MoodName);
            moodTileContent.ImageSrc = GenerateAssetPath(currentMood.Key);
            return moodTileContent;
        }

        private static string GenerateAssetPath(string assetName)
        {
            return string.Format("ms-appx:///Assets/{0}.png", assetName);
        }
    }
}