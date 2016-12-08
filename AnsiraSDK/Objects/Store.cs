using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira Store class
    /// </summary>
    public class Store
    {
        [JsonProperty(PropertyName = "storeId")]
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z_]{2,100}$", ErrorMessage = "Invalid Store Key Name")]
        public string KeyName { get; set; }

        [JsonProperty(PropertyName = "storeName")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        [RegularExpression(@"^[^\s\'\x22\,\.]+$", ErrorMessage = "Invalid Store Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }
    }
}
