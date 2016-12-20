using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
    /// <summary>
    /// Ansira Coupons API response format
    /// record as object with results as nested object
    /// </summary>
    public class ResponseV3 : Response
    {
        [JsonProperty(PropertyName = "coupon_request")]
        public User Record { get; set; }

        [JsonProperty(PropertyName = "remaining")]
        public int Remaining { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "offer_status")]
        public string OfferStatus { get; set; }

        [JsonProperty(PropertyName = "email_uri")]
        public string EmailUri { get; set; }

    }
}
