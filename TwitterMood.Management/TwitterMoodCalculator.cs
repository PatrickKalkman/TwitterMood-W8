using TwitterMood.Management.Moods;
using TwitterMood.Task;

namespace TwitterMood.Management
{
    public sealed class TwitterMoodCalculator
    {
        private const double EmotionSmoothingFactor = 0.1f;

        public void CalculateMovingAverage(MoodBase mood)
        {
            if (mood.TweetsPerHourMovingAverage <= 0)
            {
                mood.TweetsPerHourMovingAverage = mood.TweetsPerHour;
            }
            else
            {
                mood.TweetsPerHourMovingAverage = (mood.TweetsPerHourMovingAverage * (1 - EmotionSmoothingFactor)) +
                                                    (mood.TweetsPerHour*EmotionSmoothingFactor);
            }
        }
    }
}
