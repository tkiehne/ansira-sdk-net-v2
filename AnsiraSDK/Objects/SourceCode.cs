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

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z0-9_]+$", ErrorMessage = "Invalid Source Key Name")]
        public string KeyName { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "expiresAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime ExpiresAt { get; set; }

        // TODO: [eventActions][] ? Have no info on data model
    }
}
