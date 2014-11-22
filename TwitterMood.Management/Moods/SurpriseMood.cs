using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public class SurpriseMood : MoodBase
    {
        public SurpriseMood() : base("Surprise")
        {
            TemperamentRatio = TemperamentRatioDefault = 0.20f;
            MoodColor = Color.FromArgb(255, 255, 96, 0);
            MoodColorBorder = Color.FromArgb(150, 150, 48, 0);
            SuppresionFactor = 0.1;
        }

        public override void LoadIdentifyingTerms()
        {
            this.ClearTerms();
            this.AddIdentifyingTerm("wow");
            this.AddIdentifyingTerm("so excited");
            this.AddIdentifyingTerm("can't believe");
            this.AddIdentifyingTerm("wtf");
            this.AddIdentifyingTerm("unbelievable");
        }
    }
}
