using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira Pet Ownership Plan class
    /// </summary>
    public class PetOwnershipPlan
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        [RegularExpression(@"^[A-Z_]+$", ErrorMessage = "Invalid Plan Key Name")]
        public string KeyName { get; set; }

        [JsonProperty(PropertyName = "name")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        public string Name { get; set; }
    }
}