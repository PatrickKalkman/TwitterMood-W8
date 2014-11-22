using System;

namespace TwitterMood.Management
{
    public class MoodGraphDataItem
    {
        public DateTime ItemDateTime { get; set; }

        public Double HappyValue { get; set; }
        public Double EnvyValue { get; set; }
        public Double LoveValue { get; set; }
        public Double SadnessValue { get; set; }
        public Double SurpriseValue { get; set; }
        public Double AngerValue { get; set; }
        public Double FearValue { get; set; }
    }
}