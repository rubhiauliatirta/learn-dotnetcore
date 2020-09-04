using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Services
{
  public interface IUserService
  {
    Task<UserResponseDto> RegisterUserAsync(UserRegisterDto user);
    Task<UserResponseDto> LoginUserAsync(UserLoginDto user);
  }
  public class UserService: IUserService
  {
    private UserManager<ApplicationUser> _userManager;
    private IConfiguration _configuration;

    public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<UserResponseDto> LoginUserAsync(UserLoginDto user)
    {
      var loggedUser = await _userManager.FindByEmailAsync(user.Email);

      if(loggedUser == null)
      {
        return new UserResponseDto
        {
          Message = "Invalid Email/Password",
          IsSuccees = false,
        };
      }

      var result = await _userManager.CheckPasswordAsync(loggedUser, user.Password);
      var role = await _userManager.GetRolesAsync(loggedUser);

      if(!result)
      {
        return new UserResponseDto
        {
          Message = "Invalid Email/Password",
          IsSuccees = false,
        };
      }

      var claims = new []
      {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, loggedUser.Id),
        new Claim(ClaimTypes.Role , role.FirstOrDefault())
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

      var token = new JwtSecurityToken(
        issuer: _configuration["AuthSettings:Issuer"],
        audience: _configuration["AuthSettings:Audience"],
        claims: claims,
        expires: DateTime.Now.AddDays(30),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
      );

      var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

      return new UserResponseDto
      {
        Message = tokenString,
        IsSuccees = true,
      };


    }

    public async Task<UserResponseDto> RegisterUserAsync(UserRegisterDto user)
    {
      if(user == null)
      {
        throw new NullReferenceException();
      }
      if(user.Password != user.ConfirmPassword)
      {
        return new UserResponseDto
        {
          Message = "",
          IsSuccees = false
        };
      }
     

      var identityUser = new ApplicationUser
      {
        UserName = user.UserName,
        Email = user.Email,

      };

      var result = await _userManager.CreateAsync(identityUser, user.Password);
      await _userManager.AddToRoleAsync(identityUser, "User");

      if(result.Succeeded)
      {
        return new UserResponseDto
        {
          Message = "User registered successfully",
          IsSuccees = true
        };
      }

      return new UserResponseDto
      {
        Message = "User not created",
        IsSuccees = false,
        Errors = result.Errors.Select(e => e.Description)
      };
    }
  }
}