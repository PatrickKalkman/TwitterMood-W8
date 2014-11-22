using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwitterMood.Management.Moods
{
    public class TwitterResponseStorage
    {
        private readonly ILogger logger;
        private readonly Dictionary<string, RootObject> parsedTwitterResponse = new Dictionary<string, RootObject>();

        public TwitterResponseStorage(ILogger logger)
        {
            this.logger = logger;
        }

        public void Store(string key, string twitterResponse)
        {
            try
            {
                parsedTwitterResponse[key] = JsonConvert.DeserializeObject<RootObject>(twitterResponse);
            }
            catch (Exception error)
            {
                logger.Log(error.ToString());
            }
        }

        public bool TryGetValue(string key, out RootObject response)
        {
            return parsedTwitterResponse.TryGetValue(key, out response);
        }
    }
}
