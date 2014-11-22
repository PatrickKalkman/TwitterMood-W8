using System;
using System.Collections.Generic;

namespace TwitterMood.Management
{
    public sealed class TwitterMoodQueryStorage : Dictionary<string, string> 
    {
        public void Store(string key, string twitterQuery)
        {
            this[key] = twitterQuery;
        }

        public string GetQuery(string key)
        {
            string twitterQuery;

            if (this.TryGetValue(key, out twitterQuery))
            {
                return twitterQuery;
            }

            throw new ArgumentException("Key was not found in query storage", "key");
        }
    }
}