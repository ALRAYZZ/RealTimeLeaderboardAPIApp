using Microsoft.EntityFrameworkCore;
using RealTimeLeaderboardAPI.Models;

namespace RealTimeLeaderboardAPI.DataAccess
{
	public class LeaderboardContextDb : DbContext
	{
		public LeaderboardContextDb(DbContextOptions<LeaderboardContextDb> options) : base(options)
		{
		}

		public DbSet<UserModel> Users { get; set; }
		public DbSet<ScoreModel> Scores { get; set; }
	}
}
