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
        //negate: [RegularExpression(@"[^A-Za-z\s\'\x22\,\.\-]", ErrorMessage = "Invalid Last Name")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        [MinLength(1)]
        [MaxLength(100)]
        //negate: [RegularExpression(@"[^A-Za-z\s\'\x22\,\.\-]", ErrorMessage = "Invalid First Name")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "middleName")]
        [MinLength(1)]
        [MaxLength(50)]
        //negate: [RegularExpression(@"[^A-Za-z\s\'\x22\,\.\-]", ErrorMessage = "Invalid Middle Name")]
        public string MiddleName { get; set; }
        [JsonProperty(PropertyName = "dogCount")]
        public int DogCount { get; set; }
        [JsonProperty(PropertyName = "catCount")]
        public int CatCount { get; set; }
        [JsonProperty(PropertyName = "totalPets")]
        public int TotalPets { get; set; }
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
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [JsonProperty(PropertyName = "createdAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty(PropertyName = "updatedAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? UpdatedAt { get; set; }
        [JsonProperty(PropertyName = "deletedAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DeletedAt { get; set; }
        //public IList<Pet> Pets { get; set; }
        //public Dictionary<string, Subscription> Subscriptions { get; set; }

        // TODO: petOwnershipPlan
        // TODO: language
        // TODO: nationality
        // TODO: currency
        // TODO: ext[] UserExts - key/value pairs to extend user information
        // TODO: associateAtStore

        /// <summary>
        /// Subscribes the User to the specified Brand subscription
        /// </summary>
        /// <param name="brandCode">Two letter Brand code</param>
        /// <param name="brandId">Integer ID of Brand</param>
        /// <param name="sourceId">Campaign Source ID</param>
        /*public void Subscribe(string brandCode, int brandId, int sourceId)
        {
            ChangeSubscription(brandCode, brandId, sourceId, 1);
        }*/

        /// <summary>
        /// unsubscribes the User from the specified Brand subscription
        /// </summary>
        /// <param name="brandCode">Two letter Brand code</param>
        /// <param name="brandId">Integer ID of Brand</param>
        /// <param name="sourceId">Campaign Source ID</param>
        /*public void Unsubscribe(string brandCode, int brandId, int sourceId)
        {
            ChangeSubscription(brandCode, brandId, sourceId, 0);
        }*/

        /// <summary>
        /// Updates or creates Subscription object for specific Brand / Campaign
        /// </summary>
        /// <param name="brandCode">Two letter Brand code</param>
        /// <param name="brandId">Integer ID of Brand</param>
        /// <param name="sourceId">Campaign Source ID</param>
        /// <param name="status">1: subscribe; 0: unsubscribe</param>
        /*private void ChangeSubscription(string brandCode, int brandId, int sourceId, int status)
        {
            Subscription subscription = GetSubscription(brandCode);

            if (subscription == null)
            {
                subscription = new Subscription()
                {
                    BrandId = brandId,
                    SourceId = sourceId,
                    UserId = this.Id
                };
            }

            if (status == 1)
            {
                subscription.EmailStatus = "1";
            }
            else
            {
                subscription.EmailStatus = "0";
            }
            subscription.EmailDate = DateTime.Now;

            AddOrUpdateSubscription(brandCode, subscription);
        }*/

        /// <summary>
        /// Gets a Subscription for the current User
        /// </summary>
        /// <param name="brandCode">Two letter Brand code for the subscription</param>
        /// <returns>Ansira.Objects.Subscription or null if not found</returns>
        /*private Subscription GetSubscription(string brandCode)
        {
            if (Subscriptions != null && Subscriptions.ContainsKey(brandCode))
            {
                return Subscriptions[brandCode];
            }
            else
            {
                return null;
            }
        }*/

        /// <summary>
        /// Adds or replaces a Subscription for the current User
        /// </summary>
        /// <param name="brandCode">Two letter Brand code for the subscription</param>
        /// <param name="subscription">Ansira.Objects.Subscription object</param>
        /*private void AddOrUpdateSubscription(string brandCode, Subscription subscription)
        {
            if (Subscriptions == null)
            {
                Subscriptions = new Dictionary<string, Subscription>();
                Subscriptions.Add(brandCode, subscription);
            }
            else
            {
                if (Subscriptions.ContainsKey(brandCode))
                {
                    Subscriptions[brandCode] = subscription;
                }
                else
                {
                    Subscriptions.Add(brandCode, subscription);
                }
            }
        }*/

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
