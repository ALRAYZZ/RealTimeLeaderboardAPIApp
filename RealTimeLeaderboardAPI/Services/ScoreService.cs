using RealTimeLeaderboardAPI.DataAccess;
using RealTimeLeaderboardAPI.Models;
using StackExchange.Redis;
using System.Security.Claims;

namespace RealTimeLeaderboardAPI.Services
{
	public class ScoreService : IScoreService
	{
		private readonly LeaderboardContextDb _context;
		private readonly IConnectionMultiplexer _redis;

		public ScoreService(IConnectionMultiplexer redis, LeaderboardContextDb context)
		{
			_context = context;
			_redis = redis;
		}

		public async Task SubmitScore(ScoreDto scoreDto, ClaimsPrincipal userClaims)
		{
			var userId = int.Parse(userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var user = await _context.Users.FindAsync(userId);
			if (user == null)
			{
				throw new Exception("User not found.");
			}

			var score = new ScoreModel
			{
				GameName = scoreDto.GameName,
				ScoreValue = scoreDto.ScoreValue,
				UserId = userId,
				User = user
			};

			_context.Scores.Add(score);
			await _context.SaveChangesAsync();

			var db = _redis.GetDatabase();

			var currentHighScore = await db.SortedSetScoreAsync(scoreDto.GameName, user.Username);
			if (currentHighScore == null || scoreDto.ScoreValue > currentHighScore)
			{
				await db.SortedSetAddAsync(scoreDto.GameName, user.Username, scoreDto.ScoreValue);
			}
		}

		public async Task<LeaderboardDto> GetLeaderboard(string gameTitle)
		{
			var db = _redis.GetDatabase();
			var leaderboard = await db.SortedSetRangeByRankWithScoresAsync(gameTitle, 0, -1, Order.Descending);

			var leaderboardDto = new LeaderboardDto
			{
				GameTitle = gameTitle,
				LeaderboardEntries = leaderboard.Select(entry => new LeaderboardEntryDto
				{
					Username = entry.Element,
					ScoreValue = (int)entry.Score
				}).ToList()
			};

			return leaderboardDto;
		}
	}
}
