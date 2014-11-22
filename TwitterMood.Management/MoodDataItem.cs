using GalaSoft.MvvmLight;
using TwitterMood.Management.Moods;
using Windows.UI.Xaml.Media;

namespace TwitterMood.Management
{
    public class MoodDataItem : ViewModelBase
    {
        private string moodString;

        public string MoodString
        {
            get { return moodString; }
            set 
            { 
                moodString = value;
                this.RaisePropertyChanged(() => this.MoodString);
            }
        }

        private double currentValue; 

        public double CurrentValue
        {
            get
            {
                return currentValue;
            }
            set 
            { 
                currentValue = value;
                this.RaisePropertyChanged(() => this.CurrentValue);
            }
        }

        private Brush moodColor;

        public Brush MoodColor
        {
            get
            {
                return moodColor;
            }
            set
            {
                moodColor = value;
                this.RaisePropertyChanged(() => this.MoodColor);
            }
        }
    }
}