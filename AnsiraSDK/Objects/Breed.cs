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
        public string KeyName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "petType")]
        public PetType PetType { get; set; }

        // TODO: validation
        // keyName {not blank}, {length: min: 2, max: 45}, {match: /^[A-Z_]+$/}
        // name {not blank}, {length: min: 2, max: 45}, {not match: /[^A-Za-z0-9\s\'\x22\,\.]/}
    }
}