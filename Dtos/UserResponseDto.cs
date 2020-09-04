using System.Collections.Generic;

namespace LibraryAPI.Dtos
{
  public class UserResponseDto
  {
    public string Message { get; set; }
    public bool IsSuccees { get; set; }
    public IEnumerable<string> Errors { get; set; }
  }
}