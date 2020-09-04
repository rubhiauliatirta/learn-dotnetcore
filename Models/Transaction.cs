using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAnnotationsExtensions;

namespace LibraryAPI.Models
{
  public class Transaction
  {
    [Key]
    public int Id { get; set; }

    [Required, Min(1)]
    public int TotalDays { get; set;}

    [Required, Min(0)]
    public int TotalPrice { get;set; }

    public bool IsDone {get; set;}

    [Required, ForeignKey("Book")]
    public int BookId { get; set; }
    public Book Book { get; set; }

    [Required, ForeignKey("UserId")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
  }
}