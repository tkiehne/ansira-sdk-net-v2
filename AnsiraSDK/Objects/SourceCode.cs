using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira API Source Code class
    /// </summary>
    public class SourceCode
    {
        [JsonProperty(PropertyName = "key")]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "keyName")]
        public string KeyName { get; set; }
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "expiresAt")]
        public DateTime ExpiresAt { get; set; } 
    }
}
