using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
    /// <summary>
    ///  Error Reponse class
    /// </summary>
    public class ResponseError
    {
        [JsonProperty(PropertyName = "code")]
        public bool Code { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "errors")]
        public string Errors { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }
    }
}
