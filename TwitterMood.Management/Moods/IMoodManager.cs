using System.Collections.Generic;
using TwitterMood.Task;

namespace TwitterMood.Management.Moods
{
    public interface IMoodManager
    {
        void Initialize();
        void Execute();
        double GetMoodRatio(string moodKey);
        MoodGraphDataItem CreateGraphDataItem();
        void SetCurrentMoods(List<MoodDataItem> currentMoods);
        MoodBase GetDominantMood();
        string GetAdditionalSearchTerm();
    }
}