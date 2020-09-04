using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryAPI.Models;

namespace LibraryAPI.Dtos
{
  public class TransactionReadDto
  {
    public int Id { get; set; }
    public int TotalDays { get; set;}
    public int TotalPrice { get;set; }
    public bool IsDone {get; set;}
    public TransactionBookDto Book { get; set; }
    public UserReadDto User { get; set; }
  }
}