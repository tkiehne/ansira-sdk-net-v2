using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira API Subscription Channel class
    /// </summary>
    public class SubscriptionChannel
    {
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }
        [JsonProperty(PropertyName = "keyName")]
        public string KeyName { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}