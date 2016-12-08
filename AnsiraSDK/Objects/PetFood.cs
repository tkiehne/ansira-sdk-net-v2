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
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "keyName")]
        [MinLength(2)]
        [MaxLength(45)]
        public string KeyName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "foodType")]
        [MinLength(2)]
        [MaxLength(45)]
        public string FoodType { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "description")]
        [MinLength(2)]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}