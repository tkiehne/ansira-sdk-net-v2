using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira Currency class
    /// </summary>
    public class Currency
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Invalid Currency Key Name")]
        public string KeyName { get; set; } // ISO 4217 currency code

        [JsonProperty(PropertyName = "name")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        public string Name { get; set; }
    }
}