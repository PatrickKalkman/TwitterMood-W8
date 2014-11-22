using System;
using Newtonsoft.Json;
using TwitterMood.Task;

namespace TwitterMood.Management
{
    public sealed class TwitterSearchResultParser
    {
        private readonly ILogger logger;

        public TwitterSearchResultParser(ILogger logger)
        {
            this.logger = logger;
        }

        public double CalculateTweetsPerHour(string searchResult)
        {
            dynamic parsedSearchResult = JsonConvert.DeserializeObject<dynamic>(searchResult);
            if (parsedSearchResult.errors == null && parsedSearchResult.error == null)
            {
                int numberOfTweets = parsedSearchResult.statuses.Count;
                if (numberOfTweets > 1)
                {
                    DateTime latestTweetDateTime = GetLatest(parsedSearchResult);
                    DateTime earliestTweetDateTime = GetEarliest(parsedSearchResult);
                    TimeSpan difference = latestTweetDateTime - earliestTweetDateTime;
                    return numberOfTweets/difference.TotalHours;
                }
                return 0;
            }
            logger.Log(string.Format("Searchresult was error {0}", searchResult)); 
            return double.NaN;
        }

        private static DateTime GetLatest(dynamic parsedSearchResult)
        {
            DateTime latestDateTime = DateTime.MinValue;
            foreach (dynamic result in parsedSearchResult.statuses)
            {
                string currentDateTimeAsString = result.created_at;
                DateTime currentDateTime = currentDateTimeAsString.ParseTwitterTime();
                if (currentDateTime > latestDateTime)
                {
                    latestDateTime = currentDateTime;
                }
            }
            return latestDateTime;
        }

        private static DateTime GetEarliest(dynamic parsedSearchResult)
        {
            DateTime earliestDateTime = DateTime.MaxValue;
            foreach (dynamic result in parsedSearchResult.statuses)
            {
                string currentDateTimeAsString = result.created_at;
                DateTime currentDateTime = currentDateTimeAsString.ParseTwitterTime();
                if (currentDateTime < earliestDateTime)
                {
                    earliestDateTime = currentDateTime;
                }
            }
            return earliestDateTime;
        }
    }
}