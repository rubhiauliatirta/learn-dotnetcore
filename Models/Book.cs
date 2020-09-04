using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using DataAnnotationsExtensions;

namespace LibraryAPI.Models
{
  public class Book
  {
    [Key]
    public int Id { get; set; }

    [Required, MinLength(10)]
    public string Title { get; set; }

    [Required]
    public bool IsAvailable { get; set; }

    [Required]
    public int Price { get; set; }

    [JsonIgnore] 
    [IgnoreDataMember] 
    public virtual ICollection<Transaction> Transactions {get; set;}
  }
}