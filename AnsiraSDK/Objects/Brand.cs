using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
  /// <summary>
  /// Ansira API Brand class
  /// </summary>
  public class Brand
  {
    [JsonProperty(PropertyName="ID")]
    public int Id { get; set; }
    public string Name { get; set; }
    public string RichName { get; set; }
    public string Code { get; set; }
    public string Domain { get; set; }
    public string TelerxCode { get; set; }
    public string LogoName { get; set; }
    [JsonProperty(PropertyName="logoUrl")]
    public string LogoUrl { get; set; }
    [JsonProperty(PropertyName="redirect_uri")]
    public string RedirectUri { get; set; }
    //public DateTime? Created { get; set; } // killed due to invalid data in UAT (zeros)
    //public DateTime? Updated { get; set; }
    public string RegistrationURL { get; set; }
    public string HelpURL { get; set; }
    public int? ParentID { get; set; }
    public int? PetTypeId { get; set; } // used only for get[Dry|Wet]FoodBrands
  }
}
