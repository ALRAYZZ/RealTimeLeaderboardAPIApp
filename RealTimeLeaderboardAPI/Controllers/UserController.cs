using Microsoft.AspNetCore.Mvc;
using RealTimeLeaderboardAPI.Models;
using RealTimeLeaderboardAPI.Services;

namespace RealTimeLeaderboardAPI.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService=userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserRegistrationDto userDto)
		{
			try
			{
				var user = await _userService.Register(userDto);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
		{
			try
			{
				var token = await _userService.Login(userDto);
				return Ok(new { Token = token });
			}
			catch (Exception ex)
			{
				return Unauthorized(ex.Message);
			}
		}
	}
}
