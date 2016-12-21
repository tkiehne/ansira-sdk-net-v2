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
    /// Ansira Coupon API User class
    /// </summary>
    public class CouponUser
    {
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

        [JsonProperty(PropertyName = "address1")]
        [MinLength(1)]
        [MaxLength(100)]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        [MinLength(1)]
        [MaxLength(100)]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        [MinLength(1)]
        [MaxLength(45)]
        public string State { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        [MinLength(1)]
        [MaxLength(10)]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "petType")]
        [MinLength(1)]
        [MaxLength(10)]
        public string PetType { get; set; }

        [JsonProperty(PropertyName = "optIn")]
        public bool OptIn { get; set; }
    }
}
