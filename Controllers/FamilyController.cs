using CareLink_Refugee.DTOs;
using CareLink_Refugee.Mappers;
using CareLink_Refugee.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CareLink_Refugee.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyController : ControllerBase
    {
        private readonly ILogger<RefugeeController> _logger;
        private readonly RefugeeDbContext _context;

        public FamilyController(ILogger<RefugeeController> logger, RefugeeDbContext refugeeDbContext)
        {
            _context = refugeeDbContext;
            _logger = logger;
        }

        [HttpGet("GetFamilyByRefugee")]
        [ProducesResponseType(typeof(FamilyResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFamilyByRefugee([Required] Guid refugeeId)
        {
            var refugee = await _context.Refugees.FirstOrDefaultAsync(r => r.Id == refugeeId);
            if (refugee == null)
            {
                return NotFound();
            }
            var family = await _context.Families.Where(f => f.Id == refugee.FamilyId).ToListAsync();
            if (family == null || family.Count() == 0)
            {
                return NotFound();
            }
            var familyDtos = family.Select(f => RefugeeMapper.ToDto(f)).ToList();
            return Ok(familyDtos);
        }

        [HttpGet("GetAllFamilies")]
        [ProducesResponseType(typeof(List<FamilyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFamilies()
        {
            var families = await _context.Families.Include("Members").ToListAsync();
            if (families == null || families.Count() == 0)
            {
                return NotFound();
            }
            var familyDtos = families.Select(f => RefugeeMapper.ToDto(f)).ToList();

            return Ok(familyDtos);
        }

        [HttpPost("AddRefugeeToFamily")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddRefugeeToFamily([FromBody] AddRefugeeToFamilyDto dto)
        {
            var family = await _context.Families.FindAsync(dto.FamilyId);
            if (family == null)
            {
                return NotFound("Family not found.");
            }
            var refugee = await _context.Refugees.FindAsync(dto.RefugeeId);
            if (refugee == null)
            {
                return NotFound("Refugee not found");
            }

            family.Members.Add(refugee);
            await _context.SaveChangesAsync();

            return Ok($"Refugee with id [{refugee.Id}] added to family with id [{family.Id}]");
        }

        [HttpGet("GetFamilyById")]
        [ProducesResponseType(typeof(FamilyResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFamilyById([Required] Guid id)
        {
            var family = await _context.Families.Include("Members").FirstOrDefaultAsync(f => f.Id == id);
            if (family == null)
            {
                return NotFound();
            }
            var familyDto = family.ToDto();
            return Ok(familyDto);
        }

        [HttpPost("AddFamily")]
        [ProducesResponseType(typeof(CreateRefugeeResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFamily([FromBody] CreateFamilyRequestDto dto)
        {
            var family = RefugeeMapper.ToModel(dto);
            if (family == null)
            {
                return BadRequest("Family cannot be null");
            }
            _logger.LogInformation("Adding family: {@family}", family);
            if (dto.Members != null || dto.Members.Count != 0)
            {
                var refugees = await _context.Refugees
                    .Where(r => dto.Members.Contains(r.Id))
                    .ToListAsync();
                family.Members = refugees;
            }

            _context.Families.Add(family);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFamilyById), new { id = family.Id }, family.ToDto());
        }
    }

}
