using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
  /// <summary>
  /// msg as string array
  /// using Result as an array of objects
  /// </summary>
  public class ResponseV1 : Response
  {
    [JsonProperty(PropertyName = "msg")]
    public List<string> Message { get; set; }
    [JsonProperty(PropertyName = "result")]
    public List<dynamic> Result { get; set; }
  }
}
