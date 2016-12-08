using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira Language class
    /// </summary>
    public class Language
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "keyName")]
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-z]{2}$", ErrorMessage = "Invalid Language Key Name")]
        public string KeyName { get; set; } // ISO 639-1 language code

        [JsonProperty(PropertyName = "name")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(45)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "englishName")]
        public string EnglishName { get; set; }
    }
}