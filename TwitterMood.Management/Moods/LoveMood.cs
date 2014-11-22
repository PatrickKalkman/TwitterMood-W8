using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public class LoveMood : MoodBase
    {
        public LoveMood() : base("Love")
        {
            TemperamentRatio = TemperamentRatioDefault = 0.13f;
            MoodColor = Color.FromArgb(255, 255, 128, 128);
            MoodColorBorder = Color.FromArgb(150, 150, 75, 75);
            SuppresionFactor = 1;
        }

        public override void LoadIdentifyingTerms()
        {
            this.ClearTerms();
            this.AddIdentifyingTerm("i love you");
            this.AddIdentifyingTerm("i love her");
            this.AddIdentifyingTerm("i love him");
            this.AddIdentifyingTerm("all my love");
            this.AddIdentifyingTerm("i'm in love");
            this.AddIdentifyingTerm("i really love");
        }
    }
}
