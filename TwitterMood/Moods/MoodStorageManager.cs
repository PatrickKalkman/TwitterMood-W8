using System;
using System.Threading.Tasks;

namespace TwitterMood.Moods
{
    public class MoodStorageManager
    {
        public async Task<HistoricalMoodGraphData> LoadMoodDataFromStorage()
        {
            try
            {
                //var objectStorageHelper = new ObjectStorageHelper<HistoricalMoodGraphData>(StorageType.Local);
                //HistoricalMoodGraphData result = await objectStorageHelper.LoadAsync() ?? new HistoricalMoodGraphData();
                //return result;
                return new HistoricalMoodGraphData();
            }
            catch (Exception error)
            {
                throw;
            }
        }

        public async void SaveMoodDataToStorage(HistoricalMoodGraphData historicalMoodGraphData)
        {
            try
            {
                //var objectStorageHelper = new ObjectStorageHelper<HistoricalMoodGraphData>(StorageType.Local);
                //await objectStorageHelper.SaveAsync(historicalMoodGraphData);
            }
            catch (Exception error)
            {
                throw;
            }
        }
    }
}
