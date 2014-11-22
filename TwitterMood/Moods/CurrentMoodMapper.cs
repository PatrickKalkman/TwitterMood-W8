using System;
using System.Collections.Generic;
using TwitterMood.Management;
using TwitterMood.Management.Moods;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media;

namespace TwitterMood.Moods
{
    public class CurrentMoodMapper
    {
        private readonly ResourceLoader resourceLoader = new ResourceLoader();

        public List<MoodDataItem> Map(MoodGraphDataItem mostRecentMood)
        {
            var moods = new List<MoodDataItem>();

            string envy = resourceLoader.GetString("Envy");
            moods.Add(new MoodDataItem() { MoodString = envy, CurrentValue = mostRecentMood.EnvyValue, MoodColor = new SolidColorBrush(new EnvyMood().MoodColor) });
            string anger = resourceLoader.GetString("Anger");
            moods.Add(new MoodDataItem() { MoodString = anger, CurrentValue = mostRecentMood.AngerValue, MoodColor = new SolidColorBrush(new AngerMood().MoodColor) });
            string fear = resourceLoader.GetString("Fear");
            moods.Add(new MoodDataItem() { MoodString = fear, CurrentValue = mostRecentMood.FearValue, MoodColor = new SolidColorBrush(new FearMood().MoodColor) });
            string happy = resourceLoader.GetString("Happy");
            moods.Add(new MoodDataItem() { MoodString = happy, CurrentValue = mostRecentMood.HappyValue, MoodColor = new SolidColorBrush(new HappyMood().MoodColor) });
            string love = resourceLoader.GetString("Love");
            moods.Add(new MoodDataItem() { MoodString = love, CurrentValue = mostRecentMood.LoveValue, MoodColor = new SolidColorBrush(new LoveMood().MoodColor) });
            string sadness = resourceLoader.GetString("Sadness");
            moods.Add(new MoodDataItem() { MoodString = sadness, CurrentValue = mostRecentMood.SadnessValue, MoodColor = new SolidColorBrush(new SadnessMood().MoodColor) });
            string surprise = resourceLoader.GetString("Surprise");
            moods.Add(new MoodDataItem() { MoodString = surprise, CurrentValue = mostRecentMood.SurpriseValue, MoodColor = new SolidColorBrush(new SurpriseMood().MoodColor) });

            return moods;
        }

        public MoodBase Map(MoodDataItem mood)
        {
            string envy = resourceLoader.GetString("Envy");
            if (mood.MoodString == envy)
            {
                return new EnvyMood();
            }

            string anger = resourceLoader.GetString("Anger");
            if (mood.MoodString == anger)
            {
                return new AngerMood();
            }

            string fear = resourceLoader.GetString("Fear");
            if (mood.MoodString == fear)
            {
                return new FearMood();
            }

            string happy = resourceLoader.GetString("Happy");
            if (mood.MoodString == happy)
            {
                return new HappyMood();                
            }

            string love = resourceLoader.GetString("Love");
            if (mood.MoodString == love)
            {
                return new LoveMood();
            }

            string sadness = resourceLoader.GetString("Sadness");
            if (mood.MoodString == sadness)
            {
                return new SadnessMood();
            }

            string surprise = resourceLoader.GetString("Surprise");
            if (mood.MoodString == surprise)
            {
                return new SurpriseMood();
            }

            throw new ArgumentException("Unable to map the mood data item to a real mood.");
        }
    }
}
