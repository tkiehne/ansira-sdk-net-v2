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
        [JsonProperty(PropertyName = "user_id")]
        public int? UserId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }
        [JsonProperty(PropertyName = "acquisitionMethod")]
        public string AcquisitionMethod { get; set; }
        [JsonProperty(PropertyName = "discoveryMethod")]
        public string DiscoveryMethod { get; set; }
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "ageInMonths")]
        public int AgeInMonths { get; set; }
        [JsonProperty(PropertyName = "primaryBreed")]
        public Breed PrimaryBreed { get; set; }
        [JsonProperty(PropertyName = "secondaryBreed")]
        public Breed SecondaryBreed { get; set; }
        [JsonProperty(PropertyName = "petType")]
        public PetType PetType { get; set; }
        [JsonProperty(PropertyName = "foodPrefWet")]
        public PetFood FoodPrefWet { get; set; }
        [JsonProperty(PropertyName = "foodPrefDry")]
        public PetFood FoodPrefDry { get; set; }
        [JsonProperty(PropertyName = "isSterile")]
        public bool IsSterile { get; set; }
        [JsonProperty(PropertyName = "birthDate")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty(PropertyName = "adoptionDate")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DateOfAdoption { get; set; }
        [JsonProperty(PropertyName = "sourceCode")]
        public SourceCode SourceCode { get; set; }

        // TODO: Validation
        // name {length: min: 1, max: 100}, {not match: /[^A-Za-z0-9\s\'\x22\,\.\-]/}
        // imageUrl {URL, length: min: 1, max: 255}
        // size {length: min: 1, max: 45}, {not match: /[^A-Za-z0-9\s\'\x22\,\.]/}	
        // birthDate {Date YYYY-MM-DD}
        // adoptionDate {Date YYYY-MM-DD}
        // acquisitionMethod {length: min: 2, max: 45}
        // discoveryMethod {length: min: 2, max: 45}
        // discoveryMethodDetail {length: min: 2, max: 65535} 
        // color {length: min: 2, max: 45}, {not match: /[^A-Za-z0-9\s\'\x22\,\.]/}
        // gender [male|female]
    }
}