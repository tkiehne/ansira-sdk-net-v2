using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira API Brand class
    /// </summary>
    public class Brand
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "richName")]
        public string RichName { get; set; }
        [JsonProperty(PropertyName = "keyName")]
        public string KeyName { get; set; }
        [JsonProperty(PropertyName = "website")]
        public string Website { get; set; }
        [JsonProperty(PropertyName = "primaryApplication")]
        public Application PrimaryApplication { get; set; }
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
    }
}  
