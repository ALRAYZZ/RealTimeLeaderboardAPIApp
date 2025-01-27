using Microsoft.IdentityModel.Tokens;
using RealTimeLeaderboardAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeLeaderboardAPI.Services
{
	public class JwtService
	{
		private readonly IConfiguration _configuration;

		public JwtService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateJwtToken(UserModel user)
		{
			var jwtKey = _configuration["Jwt:Key"];
			var jwtIssuer = _configuration["Jwt:Issuer"];
			var jwtAudience = _configuration["Jwt:Audience"];

			if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
			{
				throw new ArgumentException("JWT configuration is missing or invalid.");
			}

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtIssuer,
				audience: jwtAudience,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(1),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
