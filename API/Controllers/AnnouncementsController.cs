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
    public async Task<IActionResult> Get ()
    {
        var listOfAnnouncements = await _context.Announcements
            .Where(a => a.IsActive == true)
            .ToListAsync();

        // TODO: Convert into DTO

        return Ok(listOfAnnouncements);
    }

    // GET api/<AnnouncementsController>/5
    [HttpGet("{id}")]
    public string Get (int id)
    {
        return "value";
    }

    [HttpPost]
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

        return Ok(created.Entity);
    }

    // PUT api/<AnnouncementsController>/5
    [HttpPut("{id}")]
    public void Put (int id, [FromBody] string value)
    {
    }

    // DELETE api/<AnnouncementsController>/5
    [HttpDelete("{id}")]
    public void Delete (int id)
    {
    }
}
