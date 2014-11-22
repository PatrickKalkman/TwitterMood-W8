using System.Collections.ObjectModel;

namespace TwitterMood.Management
{
    public interface ILogger
    {
        void Log(string format);

        ObservableCollection<string> Messages { get; }

        void Clear();
    }
}