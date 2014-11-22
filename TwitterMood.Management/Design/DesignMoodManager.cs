using System;
using System.Collections.Generic;
using TwitterMood.Management.Moods;
using TwitterMood.Task;

namespace TwitterMood.Management.Design
{
    public class DesignMoodManager : IMoodManager
    {
        public void Initialize()
        {
        }

        public void Execute()
        {
        }

        public double GetMoodRatio(string moodKey)
        {
            return 1.0;
        }

        public MoodGraphDataItem CreateGraphDataItem()
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

            return moodGraphItem;
        }

        public void SetCurrentMoods(List<MoodDataItem> currentMoods)
        {
        }

        public MoodBase GetDominantMood()
        {
            return new SurpriseMood();
        }

        public string GetAdditionalSearchTerm()
        {
            return string.Empty;
        }
    }
}
