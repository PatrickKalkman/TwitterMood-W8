using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;

namespace TwitterMood.Management.Moods
{
    public class MoodManager : IMoodManager
    {
        private readonly TwitterMoodQueryCreator twitterMoodQueryCreator;
        private readonly TwitterMoodQueryStorage twitterMoodQueryStorage;
        private readonly TwitterMoodRequest twitterMoodRequest;
        private readonly TwitterSearchResultParser twitterSearchResultParser;
        private readonly TwitterMoodCalculator twitterMoodCalculator;
        private readonly TwitterSettingsManager twitterSettingsManager;
        private readonly TwitterResponseStorage twitterResponseStorage;
        private readonly ILogger logger;
        private readonly List<MoodBase> moods = new List<MoodBase>();
        private MoodBase dominantMood;
        private TwitterMoodSettings twitterMoodSettings;

        public MoodManager(
            TwitterMoodQueryCreator twitterMoodQueryCreator, 
            TwitterMoodQueryStorage twitterMoodQueryStorage, 
            TwitterMoodRequest twitterMoodRequest,
            TwitterSearchResultParser twitterSearchResultParser, 
            TwitterMoodCalculator twitterMoodCalculator,
            TwitterSettingsManager twitterSettingsManager,
            TwitterResponseStorage twitterResponseStorage,
            ILogger logger)
        {
            this.twitterMoodQueryCreator = twitterMoodQueryCreator;
            this.twitterMoodQueryStorage = twitterMoodQueryStorage;
            this.twitterMoodRequest = twitterMoodRequest;
            this.twitterSearchResultParser = twitterSearchResultParser;
            this.twitterMoodCalculator = twitterMoodCalculator;
            this.twitterSettingsManager = twitterSettingsManager;
            this.twitterResponseStorage = twitterResponseStorage;
            this.logger = logger;
            Messenger.Default.Register<SettingsSavedEvent>(this, OnSettingsSaveEvent);
            FillMoods();
        }

        private void OnSettingsSaveEvent(SettingsSavedEvent settingsSavedEvent)
        {
            Initialize();
            ResetData();
        }

        private void FillMoods()
        {
            moods.Add(new LoveMood());
            moods.Add(new HappyMood());
            moods.Add(new SurpriseMood());
            moods.Add(new AngerMood());
            moods.Add(new EnvyMood());
            moods.Add(new SadnessMood());
            moods.Add(new FearMood());
        }

        public async void Initialize()
        {
            twitterMoodSettings = await twitterSettingsManager.GetSettingsDirect();
            CreateAndStoreQueries(twitterMoodSettings);
        }

        public async void Execute()
        {
            int numberOfMoodsRetrieved = 0;
            logger.Log(string.Format("Retrieving moods"));
            foreach (MoodBase mood in moods)
            {
                string twitterQuery = twitterMoodQueryStorage.GetQuery(mood.Key);
                string twitterResponse = await twitterMoodRequest.Execute(twitterQuery);
                twitterResponseStorage.Store(mood.Key, twitterResponse);
                mood.TweetsPerHour = mood.SuppresionFactor * twitterSearchResultParser.CalculateTweetsPerHour(twitterResponse);
                logger.Log(string.Format("MoodString {0} {1} tw/min", mood.Key, mood.TweetsPerHour));
                twitterMoodCalculator.CalculateMovingAverage(mood);
                Messenger.Default.Send(new MoodRetrieved {NumberOfMoodsRetrieved = numberOfMoodsRetrieved++ });
            }

            dominantMood = CalculateMood();
            logger.Log(string.Format("Dominant mood is {0}", dominantMood.Key));
            Messenger.Default.Send(new MoodsRetrieved());
        }

        private void ResetData()
        {
            foreach (MoodBase moodBase in moods)
            {
                moodBase.TemperamentRatio = moodBase.TemperamentRatioDefault;
                moodBase.TweetsPerHourMovingAverage = 0;
            }    
        }
        
        private MoodBase CalculateMood()
        {
            const double Smoothingfactor = 0.05f;
            double maxIncrease = -10000.0f;
            double moodSum = CalculateMoodSum();
            dominantMood = moods[0];
            foreach (MoodBase mood in moods)
            {
                mood.CurrentMoodRatio =  mood.TweetsPerHourMovingAverage / moodSum;
                double difference = mood.CurrentMoodRatio - mood.TemperamentRatio;
                difference = difference / mood.TemperamentRatio;

                logger.Log(string.Format("MoodString ratio difference {0} {1:00.00000} {2:00.00000} {3:00.00000}", mood.Key, mood.TemperamentRatio, mood.CurrentMoodRatio, difference));

                if (difference > maxIncrease)
                {
                    dominantMood = mood;
                    maxIncrease = difference;
                }
            }

            double sum = 0;
            foreach (MoodBase mood in moods)
            {
                if (mood.TemperamentRatio <= 0)
                {
                    mood.TemperamentRatio = mood.CurrentMoodRatio;
                }
                else
                {
                    mood.TemperamentRatio = (mood.TemperamentRatio * (1.0f - Smoothingfactor)) +
                        (mood.CurrentMoodRatio * Smoothingfactor);
                }

                sum += mood.TemperamentRatio;
            }

            foreach (MoodBase mood in moods)
            {
                mood.TemperamentRatio = mood.TemperamentRatio * (1.0f / sum);
            }

            return dominantMood;
        }

        private double CalculateMoodSum() 
        {
            double moodSum = 0;
            moodSum += moods.Sum(mood => mood.TweetsPerHourMovingAverage);
            return moodSum;
        }

        private void CreateAndStoreQueries(TwitterMoodSettings twitterSettings)
        {
            foreach (MoodBase mood in moods)
            {
                mood.LoadIdentifyingTerms();
                List<string> moodIdentifyingTerms = mood.GetMoodIdentifyingTerms();
                string query = twitterMoodQueryCreator.Create(moodIdentifyingTerms, twitterSettings.AdditionalSearchTerm);
                twitterMoodQueryStorage.Store(mood.Key, query);
            }
        }

        public double GetMoodRatio(string moodKey)
        {
            return (from mood in moods where mood.Key == moodKey select mood.CurrentMoodRatio).FirstOrDefault();
        }

        public MoodGraphDataItem CreateGraphDataItem()
        {
            return new MoodGraphDataItem
                        {
                            ItemDateTime = DateTime.Now,
                            EnvyValue = GetMoodRatio("Envy") * 100,
                            HappyValue = GetMoodRatio("Happy") * 100,
                            AngerValue = GetMoodRatio("Anger") * 100,
                            LoveValue = GetMoodRatio("Love") * 100,
                            SadnessValue = GetMoodRatio("Sadness") * 100,
                            SurpriseValue = GetMoodRatio("Surprise") * 100,
                            FearValue = GetMoodRatio("Fear") * 100
                        };
        }

        public void SetCurrentMoods(List<MoodDataItem> currentMoods)
        {
            foreach (MoodDataItem moodDataItem in currentMoods)
            {
                MoodBase mood = moods.Single(m => m.MoodName == moodDataItem.MoodString);
                mood.CurrentMoodRatio = moodDataItem.CurrentValue/100;
            }
        }

        public MoodBase GetDominantMood()
        {
            return dominantMood;
        }

        public string GetAdditionalSearchTerm()
        {
            return twitterMoodSettings != null ? twitterMoodSettings.AdditionalSearchTerm : string.Empty;
        }
    }
}