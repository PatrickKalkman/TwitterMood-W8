using System;
using System.Threading.Tasks;
using TwitterMood.Management;
using TwitterMood.Moods;

namespace TwitterMood.Design
{
    public class DesignMoodStorageManager : MoodStorageManager
    {
        public new async Task<HistoricalMoodGraphData> LoadMoodDataFromStorage()
        {
            var moodGraphItem = new MoodGraphDataItem();
            moodGraphItem.AngerValue = 3.0;
            moodGraphItem.EnvyValue = 5.0;
            moodGraphItem.FearValue = 0.5;
            moodGraphItem.HappyValue = 1.2;
            moodGraphItem.ItemDateTime = DateTime.Now;
            moodGraphItem.LoveValue = 6.0;
            moodGraphItem.SadnessValue = 0.04;
            moodGraphItem.SurpriseValue = 11;

            return new HistoricalMoodGraphData {moodGraphItem };
        }

        public new async void SaveMoodDataToStorage(HistoricalMoodGraphData historicalMoodGraphData)
        {
        }
    }
}
