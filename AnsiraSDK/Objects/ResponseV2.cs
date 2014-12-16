using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
  /// <summary>
  /// msg as string
  /// using Results as object
  /// </summary>
  public class ResponseV2 : Response
  {
    [JsonProperty(PropertyName = "msg")]
    public string Message { get; set; }
    [JsonProperty(PropertyName = "results")]
    public dynamic Results { get; set; }
  }
}
