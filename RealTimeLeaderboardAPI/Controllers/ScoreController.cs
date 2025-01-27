using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeLeaderboardAPI.Models;
using RealTimeLeaderboardAPI.Services;

namespace RealTimeLeaderboardAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ScoreController : ControllerBase
	{
		private readonly IScoreService _scoreService;

		public ScoreController(IScoreService scoreService)
		{
			_scoreService=scoreService;
		}

		[HttpPost("submit")]
		public async Task<IActionResult> SubmitScore([FromBody] ScoreDto scoreDto)
		{
			try
			{
				await _scoreService.SubmitScore(scoreDto, User);
				return Ok("Score submitted successfully.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("leaderboard/{gameTitle}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetLeaderboard(string gameTitle)
		{
			try
			{
				var leaderboard = await _scoreService.GetLeaderboard(gameTitle);
				return Ok(leaderboard);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
