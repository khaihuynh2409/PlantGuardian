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
    public class PlantsController : ControllerBase
    {
        private readonly PlantGuardianContext _context;

        public PlantsController(PlantGuardianContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlantDto>>> GetPlants()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var plants = await _context.Plants
                .Where(p => p.UserId == userId)
                .Select(p => new PlantDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Species = p.Species,
                    PlantType = p.PlantType,
                    DatePlanted = p.DatePlanted,
                    LastWatered = p.LastWatered,
                    SoilType = p.SoilType,
                    ImageUrl = p.ImageUrl,
                    UserId = p.UserId
                })
                .ToListAsync();

            return Ok(plants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlantDto>> GetPlant(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null) return NotFound();

            return Ok(new PlantDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Species = plant.Species,
                PlantType = plant.PlantType,
                DatePlanted = plant.DatePlanted,
                LastWatered = plant.LastWatered,
                SoilType = plant.SoilType,
                ImageUrl = plant.ImageUrl,
                UserId = plant.UserId
            });
        }

        [HttpPost]
        public async Task<ActionResult<PlantDto>> AddPlant(CreatePlantDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var plant = new Plant
            {
                Name = request.Name,
                Species = request.Species,
                PlantType = request.PlantType,
                DatePlanted = request.DatePlanted,
                SoilType = request.SoilType,
                ImageUrl = request.ImageUrl,
                UserId = userId
            };

            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlant), new { id = plant.Id }, new PlantDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Species = plant.Species,
                PlantType = plant.PlantType,
                DatePlanted = plant.DatePlanted,
                LastWatered = plant.LastWatered,
                SoilType = plant.SoilType,
                ImageUrl = plant.ImageUrl,
                UserId = plant.UserId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlant(int id, CreatePlantDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null) return NotFound();

            plant.Name = request.Name;
            plant.Species = request.Species;
            plant.PlantType = request.PlantType;
            plant.DatePlanted = request.DatePlanted;
            plant.SoilType = request.SoilType;
            if (request.ImageUrl != null) plant.ImageUrl = request.ImageUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null) return NotFound();

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
