using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Models
{
  public class ApplicationUser : IdentityUser
  {
    [JsonIgnore] 
    [IgnoreDataMember] 
    public virtual ICollection<Transaction> Transactions { get; set; }
  }
}