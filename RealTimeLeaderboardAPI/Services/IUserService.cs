using RealTimeLeaderboardAPI.Models;

namespace RealTimeLeaderboardAPI.Services
{
	public interface IUserService
	{
		Task<string> Login(UserLoginDto userDto);
		Task<UserModel> Register(UserRegistrationDto userDto);
	}
}