using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Ansira.Converters;

namespace Ansira.Objects
{
    public class Pet
    {
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        [MinLength(1)]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s\'\x22\,\.\-]+$", ErrorMessage = "Invalid Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "imageUrl")]
        [MinLength(1)]
        [MaxLength(255)]
        [Url]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "size")]
        [MinLength(1)]
        [MaxLength(45)]
        [RegularExpression(@"^[^\s\'\x22\,\.]+$", ErrorMessage = "Invalid Size")]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "acquisitionMethod")]
        [MinLength(2)]
        [MaxLength(45)]
        public string AcquisitionMethod { get; set; }

        [JsonProperty(PropertyName = "discoveryMethod")]
        [MinLength(2)]
        [MaxLength(45)]
        public string DiscoveryMethod { get; set; }

        [JsonProperty(PropertyName = "discoveryMethodDetail")]
        [MinLength(2)]
        [MaxLength(65535)]
        public string DiscoveryMethodDetail { get; set; }

        [JsonProperty(PropertyName = "color")]
        [MinLength(2)]
        [MaxLength(45)]
        [RegularExpression(@"^[^\s\'\x22\,\.]+$", ErrorMessage = "Invalid Color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "gender")]
        [RegularExpression(@"^(?i)((fe)?male)$", ErrorMessage = "Invalid Gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "ageInMonths")]
        public int? AgeInMonths { get; set; }

        [JsonProperty(PropertyName = "primaryBreed")]
        public Breed PrimaryBreed { get; set; }

        [JsonProperty(PropertyName = "secondaryBreed")]
        public Breed SecondaryBreed { get; set; }

        [JsonProperty(PropertyName = "petType")]
        public PetType Species { get; set; }

        [JsonProperty(PropertyName = "foodPrefWet")]
        public PetFood FoodPrefWet { get; set; }

        [JsonProperty(PropertyName = "foodPrefDry")]
        public PetFood FoodPrefDry { get; set; }

        [JsonProperty(PropertyName = "isSterile")]
        public bool? IsSterile { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "adoptionDate")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DateOfAdoption { get; set; }

        [JsonProperty(PropertyName = "sourceCode")]
        public SourceCode SourceCode { get; set; }
    }
}