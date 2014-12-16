using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
  /// <summary>
  /// Ansira animal breed class
  /// </summary>
  public class Breed
  {
    [JsonProperty(PropertyName="ID")]
    public int Id { get; set; }
    public string Name { get; set; }
    public int PetTypeID { get; set; }
    public string ImagePath { get; set; }
  }
}
