using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Ansira.Converters;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira User class
    /// </summary>
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "email")]
        [RegularExpression(@"^([^@\s]+)@((?:[-a-zA-Z0-9]+\.)+[a-zA-Z]{2,})$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        [MinLength(1)]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Za-z\-\']+$", ErrorMessage = "Invalid Last Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        [MinLength(1)]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Za-z\-\']+$", ErrorMessage = "Invalid First Name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        [MinLength(1)]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z\-\']+$", ErrorMessage = "Invalid Middle Name")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "dogCount")]
        public int? DogCount { get; set; }

        [JsonProperty(PropertyName = "catCount")]
        public int? CatCount { get; set; }

        [JsonProperty(PropertyName = "totalPets")]
        public int? TotalPets { get; set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        [JsonProperty(PropertyName = "sourceCode")]
        public SourceCode SourceCode { get; set; }

        [JsonProperty(PropertyName = "lastSourceCode")]
        public SourceCode LastSourceCode { get; set; }

        [JsonProperty(PropertyName = "password")]
        [MinLength(8)]
        [MaxLength(255)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(.*)$", ErrorMessage = "Insufficient Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "petOwnershipPlan")]
        public PetOwnershipPlan PetOwnershipPlan { get; set; }

        [JsonProperty(PropertyName = "language")]
        public Language Language { get; set; }

        [JsonProperty(PropertyName = "nationality")]
        public Country Nationality { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public Currency Currency { get; set; }

        [JsonProperty(PropertyName = "associateAtStore")]
        public Store AssociateAtStore { get; set; }

        [JsonProperty(PropertyName = "ext")]
        public List<dynamic> Ext { get; set; } // leaving this dynamic for now

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "deletedAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DeletedAt { get; set; }

        [JsonProperty(PropertyName = "pets")]
        public IList<Pet> Pets { get; set; }
     

        /// <summary>
        /// Add a Pet object to the current User
        /// </summary>
        /// <param name="pet">Ansira.Objects.Pet object</param>
        public void AddPet(Pet pet)
        {
            // TODO: check for Pets collection, add new item
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing Pet for the current User
        /// </summary>
        /// <param name="petId">Integer ID of the Pet</param>
        /// <param name="pet">Ansira.Objects.Pet object</param>
        public void UpdatePet(int petId, Pet pet)
        {
            // TODO: Search Pets collection for given ID, replace object
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a Pet from this User
        /// </summary>
        /// <param name="petId">Integer ID of the Pet</param>
        public void DeletePet(int petId)
        {
            // TODO: Search Pets collection for given ID, remove object
            throw new NotImplementedException();
        }
    }
}
