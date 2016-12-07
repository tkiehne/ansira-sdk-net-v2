using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Ansira.Converters;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira API Source Code class
    /// </summary>
    public class SourceCode
    {
        [JsonProperty(PropertyName = "key")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "keyName")]
        public string KeyName { get; set; }
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "expiresAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime ExpiresAt { get; set; }

        // TODO: [eventActions][]

        // TODO: validation
        // keyName {not blank}, {length: min: 2, max: 50}, {match: /^[A-Za-z0-9_]+$/}
    }
}
