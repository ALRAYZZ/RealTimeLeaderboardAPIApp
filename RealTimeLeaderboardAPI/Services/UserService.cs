using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealTimeLeaderboardAPI.DataAccess;
using RealTimeLeaderboardAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeLeaderboardAPI.Services
{
	public class UserService : IUserService
	{
		private readonly LeaderboardContextDb _context;
		private readonly IConfiguration _configuration;
		private readonly JwtService _jwtService;

		public UserService(LeaderboardContextDb context, IConfiguration configuration, JwtService jwtService)
		{
			_jwtService = jwtService;
			_context = context;
			_configuration = configuration;
		}

		public async Task<UserModel> Register(UserRegistrationDto userDto)
		{
			if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
			{
				throw new Exception("User already exists.");
			}

			var user = new UserModel()
			{
				Username = userDto.Username,
				Email = userDto.Email,
				Name = userDto.Name,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public async Task<string> Login(UserLoginDto userDto)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
			if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
			{
				throw new Exception("Invalid username or password.");
			}

			return _jwtService.GenerateJwtToken(user);
		}
	}
}
