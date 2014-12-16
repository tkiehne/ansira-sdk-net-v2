using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
  /// <summary>
  /// msg as string
  /// record as object with results as nested object
  /// </summary>
  public class ResponseV3 : Response
  {
    [JsonProperty(PropertyName = "msg")]
    public string Message { get; set; }
    [JsonProperty(PropertyName = "record")]
    public ResponseV2 Record { get; set; }
  }
}
