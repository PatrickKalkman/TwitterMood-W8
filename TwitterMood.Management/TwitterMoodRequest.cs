using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwitterMood.Management
{
    public class TwitterMoodRequest
    {
        public async Task<string> Execute(string twitterQuery)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, twitterQuery))
                {
                    request.Headers.Add("Authorization", new List<string> {"Bearer UseYourOwn"});
                    try
                    {
                        using (HttpResponseMessage responseMessage = await client.SendAsync(request))
                        {
                            return await responseMessage.Content.ReadAsStringAsync();
                        }
                    }
                    catch (Exception error)
                    {
                        var loader = new TwitterResourceLoader();
                        return loader.GetResource("TwitterMood.Management/TwitterMood/TwitterResponse");
                    }
                }
            }
        }
    }
}