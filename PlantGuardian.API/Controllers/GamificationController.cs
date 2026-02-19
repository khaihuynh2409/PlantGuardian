using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantGuardian.API.Services;
using System.Security.Claims;

namespace PlantGuardian.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GamificationController : ControllerBase
    {
        private readonly IGamificationService _gamificationService;

        public GamificationController(IGamificationService gamificationService)
        {
            _gamificationService = gamificationService;
        }

        [HttpGet("badges")]
        public async Task<ActionResult<List<string>>> GetBadges()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var badges = await _gamificationService.GetBadgesAsync(userId);
            return Ok(badges);
        }
    }
}
