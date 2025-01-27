namespace RealTimeLeaderboardAPI.Models
{
	public class ScoreModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int ScoreValue { get; set; }
		public string GameName { get; set; }
		public UserModel User { get; set; }
		public DateTime TimeStamp { get; set; }
	}
}
