using Windows.ApplicationModel.Resources.Core;

namespace TwitterMood.Management
{
    public class TwitterResourceLoader
    {
        private readonly ResourceMap resourceMap;

        public TwitterResourceLoader()
        {
            resourceMap = ResourceManager.Current.MainResourceMap;
        }

        public string GetResource(string resourceName)
        {
            return resourceMap.GetValue(resourceName).ValueAsString;
        }
    }
}
