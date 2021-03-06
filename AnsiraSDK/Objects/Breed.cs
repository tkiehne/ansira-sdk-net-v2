﻿using System;
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

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z_]{2,45}$", ErrorMessage = "Invalid Breed Key Name")]
        public string KeyName { get; set; }

        [JsonProperty(PropertyName = "name")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        [RegularExpression(@"^[^\s\'\x22\,\.]+$", ErrorMessage = "Invalid Breed Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "petType")]
        public PetType PetType { get; set; }
    }
}