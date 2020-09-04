using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Dtos
{
  public class UserRegisterDto : UserLoginDto
  {
    [Required]
    public string ConfirmPassword {get; set;}

    [Required]
    public string UserName {get; set;}
  }
}