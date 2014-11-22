using TwitterMood.Management.Moods;
using TwitterMood.Management.NotificationsExtensions;
using TwitterMood.Task.NotificationsExtensions.TileContent;

namespace TwitterMood.Task.NotificationsExtensions
{
    public sealed class MoodTileManager
    {
        private readonly MoodToTileContentConverter moodToTileContentConverter;

        public MoodTileManager(MoodToTileContentConverter moodToTileContentConverter)
        {
            this.moodToTileContentConverter = moodToTileContentConverter;
        }

        public INotificationContent Create(MoodBase currentMood)
        {
            ITileWideImageAndText01 tile = TileContentFactory.CreateTileWideImageAndText01();
            MoodTileContent tileContent = moodToTileContentConverter.GetTileContent(currentMood);
            tile.Image.Src = tileContent.ImageSrc;
            tile.TextCaptionWrap.Text = tileContent.MoodTitle;
            tile.RequireSquareContent = false;
            return tile;
        }
    }
}
