using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAnnotationsExtensions;
using LibraryAPI.Models;

namespace LibraryAPI.Dtos
{
  public class TransactionWriteDto
  {

    [Required, Min(1)]
    public int TotalDays { get; set;}

    [Required, ForeignKey("Book")]
    public int BookId { get; set; }

  }
}