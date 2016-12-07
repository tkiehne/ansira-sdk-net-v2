using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

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
        public string Address1 { get; set; }
        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty(PropertyName = "country")]
        public Country Country { get; set; }
        [JsonProperty(PropertyName = "primaryPhone")]
        public string PrimaryPhone { get; set; }
        [JsonProperty(PropertyName = "altPhone")]
        public string AltPhone { get; set; }
        [JsonProperty(PropertyName = "fax")]
        public string Fax { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public float Latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public float Longitude { get; set; }

        // TODO: validation
        // address1 {length: min: 1, max: 100}
        // [address2 {length: min: 1, max: 100}
        // city {length: min: 1, max: 100}
        // state {length: min: 1, max: 45}
        // postalCode {length: min: 1, max: 10}
        // primaryPhone {length: min: 1, max: 20}
        // altPhone {length: min: 1, max: 20}
        // fax {length: min: 1, max: 20}
    }
}
