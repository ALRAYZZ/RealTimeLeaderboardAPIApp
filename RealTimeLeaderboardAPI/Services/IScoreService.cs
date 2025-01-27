using RealTimeLeaderboardAPI.Models;
using System.Security.Claims;

namespace RealTimeLeaderboardAPI.Services
{
	public interface IScoreService
	{
		Task<LeaderboardDto> GetLeaderboard(string gameTitle);
		Task SubmitScore(ScoreDto scoreDto, ClaimsPrincipal userClaims);
	}
}