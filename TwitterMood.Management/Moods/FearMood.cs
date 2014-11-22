using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public class FearMood : MoodBase
    {
        public FearMood() : base("Fear")
        {
            TemperamentRatio = TemperamentRatioDefault = 0.10f;
            MoodColor = Color.FromArgb(255, 255, 255, 255);
            MoodColorBorder = Color.FromArgb(150, 150, 150, 150);
            SuppresionFactor = 1;
        }

        public override void LoadIdentifyingTerms()
        {
            this.ClearTerms();
            this.AddIdentifyingTerm("i'm so scared");
            this.AddIdentifyingTerm("i'm really scared");
            this.AddIdentifyingTerm("i'm terrified");
            this.AddIdentifyingTerm("i'm really afraid");
            this.AddIdentifyingTerm("so scared i");
        }
    }
}
