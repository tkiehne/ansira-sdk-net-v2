using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ansira.Objects
{
    /// <summary>
    /// Address class for Ansira Users
    /// </summary>
    public class Address
    {
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

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

        [JsonProperty(PropertyName = "country")]
        public Country Country { get; set; }

        [JsonProperty(PropertyName = "primaryPhone")]
        [MinLength(1)]
        [MaxLength(20)]
        public string PrimaryPhone { get; set; }

        [JsonProperty(PropertyName = "altPhone")]
        [MinLength(1)]
        [MaxLength(20)]
        public string AltPhone { get; set; }

        [JsonProperty(PropertyName = "fax")]
        [MinLength(1)]
        [MaxLength(20)]
        public string Fax { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }
    }
}
