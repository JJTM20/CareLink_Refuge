using CareLink_Refugee.DTOs;
using CareLink_Refugee.Models;
using CareLink_Refugee.Persistence;
using Microsoft.AspNetCore.Mvc;
using CareLink_Refugee.Mappers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CareLink_Refugee.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RefugeeController : ControllerBase
    {
        private readonly ILogger<RefugeeController> _logger;
        private readonly RefugeeDbContext _context;
        public RefugeeController(ILogger<RefugeeController> logger, RefugeeDbContext refugeeDbContext)
        {
            _context = refugeeDbContext;
            _logger = logger;
        }

        [HttpGet("GetRefugeeByName")]
        [ProducesResponseType(typeof(List<CreateRefugeeResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRefugeeByName([FromQuery] string firstName, [FromQuery] string? lastName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                return BadRequest("First name is required.");
            }

            var refugeeByName = await _context.Refugees
                .Where(r => r.FirstName.ToLower() == firstName.ToLower() && (string.IsNullOrEmpty(lastName) || r.LastName.ToLower() == lastName.ToLower()))
                .ToListAsync();

            if (!refugeeByName.Any())
            {
                return NotFound();
            }

            var refugeeDtos = refugeeByName.Select(r => RefugeeMapper.ToDto(r)).ToList();

            return Ok(refugeeDtos);
        }

        [HttpGet("GetAllRefugees")]
        [ProducesResponseType(typeof(List<CreateRefugeeResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllRefugees()
        {
            var refugees = await _context.Refugees.ToListAsync();
            return Ok(refugees);
        }
        [HttpGet("GetRefugeeById")]
        [ProducesResponseType(typeof(CreateRefugeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRefugeeById([Required] Guid id)
        {
            var refugee = await _context.Refugees.FirstOrDefaultAsync(r => r.Id == id);
            if (refugee == null)
            {
                return NotFound();
            }
            var refugeeDto = refugee.ToDto();
            return Ok(refugeeDto);
        }

        [HttpPost("AddRefugee")]
        [ProducesResponseType(typeof(CreateRefugeeResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRefugee([FromBody] CreateRefugeeRequestDto dto)
        {
            var refugee = RefugeeMapper.ToModel(dto);
            if (refugee == null)
            {
                return BadRequest("Refugee cannot be null");
            }
            _logger.LogInformation("Adding refugee: {@refugee}", refugee);
            _context.Refugees.Add(refugee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRefugeeById), new { id = refugee.Id }, refugee.ToDto());
        }

        [HttpPut("UpdateRefugee")]
        [ProducesResponseType(typeof(CreateRefugeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRefugee([FromBody] UpdateRefugeeRequestDto dto)
        {
            var refugee = await _context.Refugees
                .FirstOrDefaultAsync(r => r.Id == dto.Id);
            if (refugee == null)
            {
                return NotFound($"Refugee with Id {dto.Id} not found.");
            }
            refugee = dto.ToModel(refugee);
            await _context.SaveChangesAsync();

            return Ok(RefugeeMapper.ToDto(refugee));
        }

        [HttpDelete("DeleteRefugeeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRefugee([Required] Guid id)
        {
            var refugee = await _context.Refugees.FirstOrDefaultAsync(r => r.Id == id);
            if (refugee == null)
            {
                return NotFound();
            }
            _context.Refugees.Remove(refugee);
            await _context.SaveChangesAsync();
            return Ok($"Refugee with id [{refugee.Id}] deleted.");
        }

        [HttpGet("GetRefugeesByAccomodationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRefugeesByAccomodationId([Required] Guid accomodationId)
        {
            var refugees = await _context.Refugees.Where(r => r.AccomodationId == accomodationId).ToListAsync();
            if (refugees == null || refugees.Count == 0)
            {
                return NotFound();
            }
            var refugeeDtos = refugees.Select(r => RefugeeMapper.ToDto(r)).ToList();
            return Ok(refugeeDtos);
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

        [HttpGet("GetAllAccomodations")]
        [ProducesResponseType(typeof(List<AccomodationResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAccomodations()
        {
            var accomodations = await _context.Shelters.ToListAsync();
            if (accomodations == null || accomodations.Count() == 0)
            {
                return NotFound();
            }

            var accomodationDtos = accomodations.Select(a => RefugeeMapper.ToDto(a)).ToList();

            return Ok(accomodationDtos);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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
    }
}
