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
        public string Name { get; set; }
        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        public string KeyName { get; set; } // ISO 3166-1 alpha-2 country code

        // TODO: validation
        // name {not blank}, {length: min: 2, max: 45}	
        // keyName {not blank}, {length: min: 2, max: 5}, {match: /^[A-Z_]+$/}
    }
}