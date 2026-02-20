using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantGuardian.API.Data;
using PlantGuardian.API.DTOs;
using PlantGuardian.API.Models;
using PlantGuardian.API.Services;
using System.Security.Claims;

namespace PlantGuardian.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BeanController : ControllerBase
    {
        private readonly PlantGuardianContext _context;
        private readonly IBeanCareService _beanCareService;

        public BeanController(PlantGuardianContext context, IBeanCareService beanCareService)
        {
            _context = context;
            _beanCareService = beanCareService;
        }

        /// <summary>
        /// Lấy hồ sơ chăm sóc cho một loại đậu.
        /// PlantType hợp lệ: BlackBean, Soybean, FavaBean
        /// </summary>
        [HttpGet("care-profile/{plantType}")]
        public ActionResult<BeanCareProfileDto> GetCareProfile(string plantType)
        {
            var profile = _beanCareService.GetCareProfile(plantType);
            if (profile == null)
                return NotFound(new { message = $"Không tìm thấy hồ sơ cho loại đậu: {plantType}. Hợp lệ: BlackBean, Soybean, FavaBean" });

            return Ok(profile);
        }

        /// <summary>
        /// Lấy toàn bộ nhật ký phát triển của một cây.
        /// </summary>
        [HttpGet("diary/{plantId}")]
        public async Task<ActionResult<List<BeanDiaryEntryDto>>> GetDiary(int plantId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Verify plant belongs to the current user
            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == plantId && p.UserId == userId);
            if (plant == null) return NotFound(new { message = "Không tìm thấy cây." });

            var entries = await _context.BeanDiaryEntries
                .Where(e => e.PlantId == plantId)
                .OrderByDescending(e => e.EntryDate)
                .Select(e => new BeanDiaryEntryDto
                {
                    Id = e.Id,
                    PlantId = e.PlantId,
                    EntryDate = e.EntryDate,
                    GrowthStage = e.GrowthStage,
                    Notes = e.Notes,
                    HeightCm = e.HeightCm,
                    HealthRating = e.HealthRating
                })
                .ToListAsync();

            return Ok(entries);
        }

        /// <summary>
        /// Thêm một entry nhật ký mới cho cây đậu.
        /// </summary>
        [HttpPost("diary/{plantId}")]
        public async Task<ActionResult<BeanDiaryEntryDto>> AddDiaryEntry(int plantId, CreateBeanDiaryEntryDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == plantId && p.UserId == userId);
            if (plant == null) return NotFound(new { message = "Không tìm thấy cây." });

            var validStages = new[] { "Seedling", "Vegetative", "Flowering", "Podding", "Harvest" };
            if (!validStages.Contains(request.GrowthStage))
                return BadRequest(new { message = $"GrowthStage không hợp lệ. Hợp lệ: {string.Join(", ", validStages)}" });

            if (request.HealthRating < 1 || request.HealthRating > 5)
                return BadRequest(new { message = "HealthRating phải từ 1 đến 5." });

            var entry = new BeanDiaryEntry
            {
                PlantId = plantId,
                UserId = userId,
                EntryDate = request.EntryDate,
                GrowthStage = request.GrowthStage,
                Notes = request.Notes,
                HeightCm = request.HeightCm,
                HealthRating = request.HealthRating
            };

            _context.BeanDiaryEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDiary), new { plantId }, new BeanDiaryEntryDto
            {
                Id = entry.Id,
                PlantId = entry.PlantId,
                EntryDate = entry.EntryDate,
                GrowthStage = entry.GrowthStage,
                Notes = entry.Notes,
                HeightCm = entry.HeightCm,
                HealthRating = entry.HealthRating
            });
        }

        /// <summary>
        /// Lấy số ngày còn lại đến lần tưới nước tiếp theo.
        /// </summary>
        [HttpGet("watering-schedule/{plantId}")]
        public async Task<ActionResult> GetWateringSchedule(int plantId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == plantId && p.UserId == userId);
            if (plant == null) return NotFound(new { message = "Không tìm thấy cây." });

            var daysLeft = _beanCareService.GetDaysUntilNextWatering(plant.PlantType, plant.LastWatered);
            var profile = _beanCareService.GetCareProfile(plant.PlantType);

            return Ok(new
            {
                plantId = plant.Id,
                plantName = plant.Name,
                plantType = plant.PlantType,
                lastWatered = plant.LastWatered,
                daysUntilNextWatering = daysLeft,
                wateringFrequencyDays = profile?.WateringFrequencyDays,
                needsWateringNow = daysLeft == 0,
                message = daysLeft == 0
                    ? "⚠️ Cây cần được tưới nước ngay hôm nay!"
                    : $"✅ Còn {daysLeft} ngày là tưới nước tiếp theo."
            });
        }

        /// <summary>
        /// Lấy danh sách các loại đậu được hỗ trợ.
        /// </summary>
        [HttpGet("supported-types")]
        [AllowAnonymous]
        public ActionResult GetSupportedBeanTypes()
        {
            return Ok(new[]
            {
                new { key = "BlackBean", displayName = "Đậu Đen", emoji = "🖤" },
                new { key = "Soybean",  displayName = "Đậu Nành", emoji = "🟡" },
                new { key = "FavaBean", displayName = "Đậu Rộng", emoji = "🟢" }
            });
        }
    }
}
