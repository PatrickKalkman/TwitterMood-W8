using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public abstract class MoodBase
    {
        private readonly string mood;

        private readonly string moodName;

        private readonly List<string> moodIdentifyingTerms = new List<string>();

        public double TemperamentRatio { get; set; }

        public double TemperamentRatioDefault { get; set; }

        public double TweetsPerHour { get; set; }

        public double TweetsPerHourMovingAverage { get; set; }

        public double CurrentMoodRatio { get; set; }

        public double SuppresionFactor { get; set; }

        public Color MoodColor { get; set; }

        public Color MoodColorBorder { get; set; }

        protected MoodBase(string mood)
        {
            var loader = new ResourceLoader();
            this.mood = mood;
            this.moodName = loader.GetString(mood);
        }

        public string Key
        {
            get { return mood; }
        }

        public string MoodName
        {
            get { return moodName;  }
        }

        public void AddIdentifyingTerm(string identifyingTerm)
        {
            this.moodIdentifyingTerms.Add(identifyingTerm);
        }

        public abstract void LoadIdentifyingTerms();

        public List<string> GetMoodIdentifyingTerms()
        {
            return moodIdentifyingTerms;
        }

        protected void ClearTerms()
        {
            moodIdentifyingTerms.Clear();
        }

    }
}
