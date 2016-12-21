using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Ansira.Converters;

namespace Ansira.Objects
{
    public class Subscription
    {
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "subscriptionChannel")]
        public SubscriptionChannel SubscriptionChannel { get; set; }

        [JsonProperty(PropertyName = "brand")]
        public Brand Brand { get; set; }

        [JsonProperty(PropertyName = "sourceCode")]
        public SourceCode SourceCode { get; set; }

        [JsonProperty(PropertyName = "optedInAt")]
        [JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? OptedInAt { get; set; }
    }
}