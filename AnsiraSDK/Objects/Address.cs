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
    [JsonProperty(PropertyName = "ID")]
    public int? Id { get; set; }
    public string Address1 {get; set;}
    public string Address2 {get; set;}
    public string City {get; set;}
    public string State {get; set;}
    public string Zip {get; set;}
    public string Phone { get; set; }
  }
}
