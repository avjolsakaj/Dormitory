using API.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    // Connect with database using context
    private readonly RestApiDormitoryContext _context;

    public RoomsController (RestApiDormitoryContext restApiDormitoryContext)
    {
        _context = restApiDormitoryContext;
    }

    /// <summary>
    /// Endpoint: Get list of rooms
    /// </summary>
    /// <returns>Get list of rooms</returns>
    [HttpGet("list")]
    public async Task<IActionResult> Get ()
    {
        var listOfRooms = await _context.Rooms.ToListAsync();

        // TODO: Convert into DTO

        return Ok(listOfRooms);
    }
}
