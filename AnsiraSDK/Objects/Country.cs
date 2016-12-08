using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Country class for Ansira Address
    /// </summary>
    public class Country
    {
        [JsonProperty(PropertyName = "name")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Invalid Country Key Name")]
        public string KeyName { get; set; } // ISO 3166-1 alpha-2 country code
    }
}