using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public class EnvyMood : MoodBase
    {
        public EnvyMood() : base("Envy")
        {
            TemperamentRatio = TemperamentRatioDefault = 0.16f;
            MoodColor = Color.FromArgb(255, 0, 255, 0);
            MoodColorBorder = Color.FromArgb(150, 0, 150, 0);
            SuppresionFactor = 1;
        }

        public override void LoadIdentifyingTerms()
        {
            this.ClearTerms();
            this.AddIdentifyingTerm("i wish i");
            this.AddIdentifyingTerm("i'm envious");
            this.AddIdentifyingTerm("i'm jealous");
            this.AddIdentifyingTerm("i want to be");
            this.AddIdentifyingTerm("why can't i");
        }
    }
}
