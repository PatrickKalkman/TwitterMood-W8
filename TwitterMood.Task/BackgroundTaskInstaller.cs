using System;
using Windows.ApplicationModel.Background;

namespace TwitterMood.Task
{
    public sealed class BackgroundTaskInstaller
    {
        private const string TaskName = "TileUpdaterBackgroundTask";
        private const string TaskEntry = "TwitterMood.Task.TileUpdaterBackgroundTask";

        public async void Install()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                result == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == TaskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                var builder = new BackgroundTaskBuilder();
                builder.Name = TaskName;
                builder.TaskEntryPoint = TaskEntry;
                builder.SetTrigger(new TimeTrigger(15, false));
                var registration = builder.Register();
            }
        }
    }
}
