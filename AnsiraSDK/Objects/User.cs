using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ansira.Objects
{
  /// <summary>
  /// Ansira User class
  /// </summary>
  public class User
  {
    [JsonProperty(PropertyName="ID")]
    public int? Id { get; set; }
    [JsonProperty(PropertyName="UUID")]
    public string Uuid { get; set; }
    public string Email {get; set;}
    [JsonProperty(PropertyName = "SourceID")]
    public int? SourceId {get; set;}
    public string FamilyName {get; set;}
    public string MiddleName {get; set;}
    public string GivenName {get; set;}
    public Address Address { get; set; }
    public DateTime? BirthDay {get; set;}
    public string Gender {get; set;}
    public string DisplayName {get; set;}
    public int? OriginationID {get; set;}
    public int? LastSourceID {get; set;}
    public int? RegistrationSourceId {get; set;}
    public string CurrentLocation {get; set;}
    public DateTime? LastLogin { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    /* TODO: Subscriptions & Pets */
  }
}
