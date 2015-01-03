using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Ansira.Objects
{
  public class Subscription
  {
    [JsonProperty(PropertyName = "ID")]
    public int? Id { get; set; }
    [JsonProperty(PropertyName = "UserID")]
    public int? UserId { get; set; }
    [Required]
    [JsonProperty(PropertyName = "BrandID")]
    public int BrandId { get; set; }
    [Required]
    [JsonProperty(PropertyName = "SourceID")]
    public int SourceId { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string EmailStatus { get; set; }
    [Required]
    public DateTime EmailDate { get; set; }
    public string MobileStatus { get; set; }
    public DateTime? MobileDate { get; set; }
    public int EmailTriggerFlag { get; set; }
  }
}
