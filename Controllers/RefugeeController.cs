using CareLink_Refugee.Persistence;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get()
        {
            return Ok("Welcome to the Refuge API!");
        }
        [HttpGet("GetAllRefugees")]
        public IActionResult GetAllRefugees()
        {
            var refugees = _context.Refugees.ToList();
            return Ok(refugees);
        }
    }
}
