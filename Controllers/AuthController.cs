using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Dtos;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {
    private IUserService _userService;
    public AuthController(IUserService userService)
    {
      _userService = userService;
    }


    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(UserRegisterDto user)
    {
      if (ModelState.IsValid)
      {
        var result = await _userService.RegisterUserAsync(user);

        if (result.IsSuccees)
        {
          return Ok(result);
        }

        return BadRequest(result);
      }

      return BadRequest("Some property are not valid");
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync(UserLoginDto user)
    {
      if (ModelState.IsValid)
      {
        var result = await _userService.LoginUserAsync(user);

        if (result.IsSuccees)
        {
          return Ok(result);
        }

        return BadRequest(result);
      }

      return BadRequest("Some property are not valid");
    }
  }
}
