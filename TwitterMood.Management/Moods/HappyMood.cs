using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public class HappyMood : MoodBase
    {
        public HappyMood() : base("Happy")
        {
            TemperamentRatio = TemperamentRatioDefault = 0.15f;
            MoodColor = Color.FromArgb(255, 255, 255, 0);
            MoodColorBorder = Color.FromArgb(150, 150, 150, 0);
            SuppresionFactor = 1;
        }

        public override void LoadIdentifyingTerms()
        {
            this.ClearTerms();
            this.AddIdentifyingTerm("happiest");
            this.AddIdentifyingTerm("so happy");
            this.AddIdentifyingTerm("so excited");
            this.AddIdentifyingTerm("i'm happy");
            this.AddIdentifyingTerm("woot");
            this.AddIdentifyingTerm("w00t");
        }
    }
}
