using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira Pet Type class
    /// </summary>
    public class PetType
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "keyName")]
        [MinLength(2)]
        [MaxLength(45)]
        [RegularExpression(@"^[A-Z_]+$", ErrorMessage = "Invalid Pet Type Key Name")]
        public string KeyName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "name")]
        [MinLength(2)]
        [MaxLength(45)]
        public string Name { get; set; }
    }
}