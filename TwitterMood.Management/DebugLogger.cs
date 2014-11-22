using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace TwitterMood.Management
{
    public class DebugLogger : ViewModelBase, ILogger
    {
        private readonly ObservableCollection<string> logMessages = new ObservableCollection<string>();

        public ObservableCollection<string> Messages
        {
            get { return logMessages; }
        }

        public void Clear()
        {
            Messages.Clear();
        }

        public void Log(string message)
        {
            logMessages.Add(message);
        }
    }
}
