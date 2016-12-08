using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira Pet Food class
    /// </summary>
    public class PetFood
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        public string KeyName { get; set; }

        [JsonProperty(PropertyName = "foodType")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        public string FoodType { get; set; }

        [JsonProperty(PropertyName = "description")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}