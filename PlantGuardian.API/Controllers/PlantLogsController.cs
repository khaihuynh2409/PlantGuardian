using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantGuardian.API.Data;
using PlantGuardian.API.DTOs;
using PlantGuardian.API.Models;
using System.Security.Claims;

namespace PlantGuardian.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlantLogsController : ControllerBase
    {
        private readonly PlantGuardianContext _context;

        public PlantLogsController(PlantGuardianContext context)
        {
            _context = context;
        }

        [HttpGet("{plantId}")]
        public async Task<ActionResult<List<PlantLogDto>>> GetLogs(int plantId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            // Verify plant belongs to user
            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == plantId && p.UserId == userId);
            if (plant == null) return NotFound("Plant not found or access denied.");

            var logs = await _context.PlantLogs
                .Where(l => l.PlantId == plantId)
                .OrderByDescending(l => l.LogDate)
                .Select(l => new PlantLogDto
                {
                    Id = l.Id,
                    LogDate = l.LogDate,
                    ImageUrl = l.ImageUrl,
                    SoilMoistureStatus = l.SoilMoistureStatus,
                    HealthStatus = l.HealthStatus,
                    Notes = l.Notes,
                    PlantId = l.PlantId
                })
                .ToListAsync();

            return Ok(logs);
        }

        [HttpPost]
        public async Task<ActionResult<PlantLogDto>> AddLog(CreatePlantLogDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            // Verify plant belongs to user
            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == request.PlantId && p.UserId == userId);
            if (plant == null) return NotFound("Plant not found or access denied.");

            var log = new PlantLog
            {
                PlantId = request.PlantId,
                SoilMoistureStatus = request.SoilMoistureStatus,
                HealthStatus = request.HealthStatus,
                Notes = request.Notes,
                ImageUrl = request.ImageUrl,
                LogDate = DateTime.UtcNow
            };

            _context.PlantLogs.Add(log);
            
            // Update plant's last watered if status implies water? 
            // For now just logging.
            
            await _context.SaveChangesAsync();

            return Ok(new PlantLogDto
            {
                Id = log.Id,
                LogDate = log.LogDate,
                ImageUrl = log.ImageUrl,
                SoilMoistureStatus = log.SoilMoistureStatus,
                HealthStatus = log.HealthStatus,
                Notes = log.Notes,
                PlantId = log.PlantId
            });
        }
    }
}
