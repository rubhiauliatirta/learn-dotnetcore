using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace LibraryAPI.Dtos
{
  public class BookWriteDto
  {
    [Required, MinLength(10)]
    public string Title { get; set; }

    [Required, Min(0), Max(10000)]
    public int Price { get; set; }

  }
}