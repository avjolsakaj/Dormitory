using API.DAL;
using API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementsController : ControllerBase
{
    private readonly RestApiDormitoryContext _context;

    public AnnouncementsController (RestApiDormitoryContext restApiDormitoryContext)
    {
        _context = restApiDormitoryContext;
    }

    /// <summary>
    /// Get list of announcements
    /// </summary>
    /// <returns>List of Announcements</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Announcement>))]
    public async Task<IActionResult> Get ()
    {
        var listOfAnnouncements = await _context.Announcements
            .Where(a => a.IsActive == true)
            .ToListAsync();

        // TODO: Convert into DTO

        return Ok(listOfAnnouncements);
    }

    /// <summary>
    /// Get announcement by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Announcement</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Announcement))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> Get (int id)
    {
        var announcement = await _context.Announcements
            .FirstOrDefaultAsync(a => a.IsActive == true && a.Id == id);

        if (announcement == null)
        {
            return NotFound($"Announcement {id} not found!");
        }

        return Ok(announcement);
    }

    /// <summary>
    /// Create an announcement
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Created Announcement</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Announcement))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Post ([FromBody] CreateAnnouncementDTO request)
    {
        if (string.IsNullOrEmpty(request.Title))
        {
            return BadRequest("Title is required!");
        }

        if (string.IsNullOrEmpty(request.Description))
        {
            return BadRequest("Description is required!");
        }

        if (request.RoomId <= 0)
        {
            return BadRequest("Room number is not valid!");
        }

        var roomExists = await _context.Rooms.AnyAsync(r => r.Id == request.RoomId);
        if (!roomExists)
        {
            return BadRequest($"Room {request.RoomId}, doesn't exist!");
        }

        var hasActiveAnnouncementForRoom = await _context.Announcements
            .AnyAsync(x => x.IsActive == true && x.RoomId == request.RoomId);
        if (hasActiveAnnouncementForRoom)
        {
            return BadRequest("There is already active announcement for this room!");
        }

        var announcementToBeCreated = new Announcement
        {
            Title = request.Title,
            Description = request.Description,
            PublishedDate = DateTime.Now,
            IsActive = true,
            RoomId = request.RoomId
        };

        var created = await _context.Announcements.AddAsync(announcementToBeCreated);

        await _context.SaveChangesAsync();

        // TODO: Convert into DTO

        return Created(nameof(Get), created.Entity);
    }

    /// <summary>
    /// Update announcement by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns>Updated announcement</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Announcement))]
    public async Task<IActionResult> Put (int id, [FromBody] UpdateAnnouncementDTO request)
    {
        if (string.IsNullOrEmpty(request.Title))
        {
            return BadRequest("Title is required!");
        }

        if (string.IsNullOrEmpty(request.Description))
        {
            return BadRequest("Description is required!");
        }

        // Get announcement in database
        var announcement = await _context.Announcements
            .FirstOrDefaultAsync(a => a.IsActive == true && a.Id == id);

        if (announcement == null)
        {
            return BadRequest($"Announcement {id} doesn't exist!");
        }

        // Update announcement
        announcement.Title = request.Title;
        announcement.Description = request.Description;

        // Save database
        await _context.SaveChangesAsync();

        // Return updated announcement
        return Ok(announcement);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete (int id)
    {
        // Get announcement by Id and isActive == true
        var existingAnnouncement = await _context.Announcements
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);

        // If announcement does not exist, return
        if (existingAnnouncement == null)
        {
            return NoContent();
        }

        // Set isActive to false
        existingAnnouncement.IsActive = false;

        // Save changes
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
