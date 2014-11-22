using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public class SadnessMood : MoodBase
    {
        public SadnessMood() : base("Sadness")
        {
            TemperamentRatio = TemperamentRatioDefault = 0.12f;
            MoodColor = Color.FromArgb(255, 0, 0, 255);
            MoodColorBorder = Color.FromArgb(150, 0, 0, 150);
            SuppresionFactor = 1;
        }

        public override void LoadIdentifyingTerms()
        {
            this.ClearTerms();
            this.AddIdentifyingTerm("i'm so sad");
            this.AddIdentifyingTerm("i'm heartbroken");
            this.AddIdentifyingTerm("i'm so upset");
            this.AddIdentifyingTerm("i'm depressed");
            this.AddIdentifyingTerm("i can't stop crying");
        }
    }
}
