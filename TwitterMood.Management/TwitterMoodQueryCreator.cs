using System.Collections.Generic;
using System.Text;

namespace TwitterMood.Management
{
    public sealed class TwitterMoodQueryCreator
    {
        private const string BaseUri = @"https://api.twitter.com/1.1/search/tweets.json?q=";
        private const string EndUri = @"&rpp=40&result_type=recent";

        public string Create(List<string> moodIdentifyingTerms, string additionalSearchTerm)
        {
            // For example, http://search.twitter.com/search.json?q=blue%20angels&rpp=30&include_entities=true&result_type=mixed

            var twitterQuery = new StringBuilder();

            foreach (string identifyingTerm in moodIdentifyingTerms)
            {
                if (!string.IsNullOrEmpty(additionalSearchTerm))
                {
                    twitterQuery.AppendFormat("\"{0} {1}\"%20OR%20", identifyingTerm, additionalSearchTerm.Replace(" ", "%20"));
                }
                else
                {
                    twitterQuery.AppendFormat("\"{0}\"%20OR%20", identifyingTerm);
                }
            }

            twitterQuery.Remove(twitterQuery.Length - 8, 8);

            return string.Format("{0}{1}{2}", BaseUri, twitterQuery, EndUri);
        }
    }
}