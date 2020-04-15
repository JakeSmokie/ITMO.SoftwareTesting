using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ITMO.SoftwareTesting.Dates.Contracts.Models
{
    public class IntegrationEventDetails
    {
        public class TimeRange
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }

        public int Id { get; set; }

        public List<TimeRange> Dates { get; set; }
        public string Description { get; set; }

        [JsonProperty("body_text")] 
        public string BodyText { get; set; }

        public List<string> Images { get; set; }
    }
}