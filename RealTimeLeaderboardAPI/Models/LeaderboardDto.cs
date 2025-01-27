namespace RealTimeLeaderboardAPI.Models
{
	public class LeaderboardDto
	{
		public string GameTitle { get; set; }
		public List<LeaderboardEntryDto> LeaderboardEntries { get; set; }
	}
}
