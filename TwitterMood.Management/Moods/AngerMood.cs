using Windows.UI;

namespace TwitterMood.Management.Moods
{
    public class AngerMood : MoodBase
    {
        public AngerMood() : base("Anger")
        {
            TemperamentRatio = TemperamentRatioDefault = 0.14f;
            MoodColor = Color.FromArgb(255, 255, 0, 0);
            MoodColorBorder = Color.FromArgb(150, 150, 0, 0);
            SuppresionFactor = 1;
        }

        public override void LoadIdentifyingTerms()
        {
            this.ClearTerms();
            this.AddIdentifyingTerm("i hate");
            this.AddIdentifyingTerm("really angry");
            this.AddIdentifyingTerm("i am mad");
            this.AddIdentifyingTerm("really hate");
            this.AddIdentifyingTerm("so angry");
        }
    }
}
