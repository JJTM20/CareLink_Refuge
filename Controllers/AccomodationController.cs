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
    public class AccomodationController : ControllerBase
    {
        private readonly ILogger<RefugeeController> _logger;
        private readonly RefugeeDbContext _context;

        public AccomodationController(ILogger<RefugeeController> logger, RefugeeDbContext refugeeDbContext)
        {
            _context = refugeeDbContext;
            _logger = logger;
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

        [HttpPost("AddAccomodation")]
        [ProducesResponseType(typeof(AccomodationResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAccomodation([FromBody] CreateAccomodationRequestDto dto)
        {
            var accomodation = RefugeeMapper.ToModel(dto);
            if (accomodation == null)
            {
                return BadRequest("Accomodation cannot be null");
            }
            _logger.LogInformation("Adding accomodation: {@accomodation}", accomodation);
            _context.Shelters.Add(accomodation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllAccomodations), new { id = accomodation.Id }, accomodation.ToDto());
        }

        [HttpPut("AddRefugeeToAccomodation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddRefugeeToAccomodation([FromBody] AddRefugeeToAccomodationDto dto)
        {
            var accomodation = await _context.Shelters.FindAsync(dto.AccomodationId);
            if (accomodation == null)
            {
                return NotFound("Accomodation not found.");
            }
            var refugee = await _context.Refugees.FindAsync(dto.RefugeeId);
            if (refugee == null)
            {
                return NotFound("Refugee not found");
            }
            accomodation.Refugees.Add(refugee);
            await _context.SaveChangesAsync();
            return Ok($"Refugee with id [{refugee.Id}] added to accomodation with id [{accomodation.Id}]");
        }

        [HttpGet("GetAccomodationById")]
        [ProducesResponseType(typeof(AccomodationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccomodationById([Required] Guid id)
        {
            var accomodation = await _context.Shelters.FirstOrDefaultAsync(a => a.Id == id);
            if (accomodation == null)
            {
                return NotFound();
            }
            var accomodationDto = accomodation.ToDto();
            return Ok(accomodationDto);
        }

    }
}
