using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira API Application class
    /// </summary>
    public class Application
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "brand")]
        public Brand Brand { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "stageDomain")]
        public string StageDomain { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
    }
}
