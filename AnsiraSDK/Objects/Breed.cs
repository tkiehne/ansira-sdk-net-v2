using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira animal breed class
    /// </summary>
    public class Breed
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "keyName")]
        [RegularExpression(@"^[A-Z_]{2,45}$", ErrorMessage = "Invalid Breed Key Name")]
        public string KeyName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "name")]
        [MinLength(2)]
        [MaxLength(45)]
        // negate [RegularExpression(@"[^A-Za-z0-9\s\'\x22\,\.]", ErrorMessage = "Invalid Breed Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "petType")]
        public PetType PetType { get; set; }
    }
}