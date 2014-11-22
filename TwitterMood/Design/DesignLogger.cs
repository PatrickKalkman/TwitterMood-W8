using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using TwitterMood.Management;

namespace TwitterMood.Design
{
    public class DesignLogger : ViewModelBase, ILogger
    {
        public DesignLogger()
        {
            this.Messages.Add("Log message number 1");
            this.Messages.Add("Log message number 2");
            this.Messages.Add("Log message number 3");
            this.Messages.Add("Log message number 4");
            this.Messages.Add("Log message number 5");
        }

        public void Log(string format)
        {
        }

        readonly ObservableCollection<string> logMessages = new ObservableCollection<string>();

        public ObservableCollection<string> Messages
        {
            get { return logMessages; }
        }

        public void Clear()
        {
        }
    }
}