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
        public string KeyName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "foodType")]
        public string FoodType { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        // TODO: validation
        // keyName {not blank}, {length: min: 2, max: 45}
        // foodType {not blank}, {length: min: 2, max: 45}
        // description {not blank}, {length: min: 2, max: 255}
    }
}