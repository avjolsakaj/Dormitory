using Dormitory.DAL;

namespace Dormitory.BLL;

// AnnouncementService
public class AnnouncementManagement
{
    public static void AddAnnouncemnt ()
    {
        using var context = new FindRooMateContext();

        Console.WriteLine("Enter announcement title:");
        var title = Console.ReadLine();

        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine("Title is required.");
            _ = Console.ReadLine();
            return;
        }

        Console.WriteLine("Enter announcement description:");
        var description = Console.ReadLine();

        if (string.IsNullOrEmpty(description))
        {
            Console.WriteLine("Description is required.");
            _ = Console.ReadLine();
            return;
        }

        Console.WriteLine("List of rooms: ");
        var existingRooms = context.Rooms.ToList();
        foreach (var room in existingRooms)
        {
            Console.WriteLine($"Id: {room.Id}, Room: {room.Name}, Capacity: {room.Capacity}");
        }

        Console.WriteLine("Enter room Id:");
        _ = int.TryParse(Console.ReadLine(), out var roomId);

        var roomExists = existingRooms.Any(room => room.Id == roomId);
        if (!roomExists)
        {
            Console.WriteLine($"Room {roomId}, does not exist!");
            _ = Console.ReadLine();
            return;
        }

        var hasActiveAccouncemntForRoom = context.Announcements
            .Any(x => x.IsActive == true && x.RoomId == roomId);

        if (hasActiveAccouncemntForRoom)
        {
            Console.WriteLine("There is already active announcement for this room.");
            _ = Console.ReadLine();
            return;
        }

        var created = context.Announcements.Add(new Announcement
        {
            Title = title,
            Description = description,
            PublishedDate = DateTime.Now,
            IsActive = true,
            RoomId = roomId
        });

        Console.WriteLine($"Announcements {created.Entity.Id} - {created.Entity.Title} is added.");
        // Console.WriteLine($"Announcements {title} is added.");

        _ = context.SaveChanges();
    }

    public static void TerminateAnnouncemnt ()
    {
        using var context = new FindRooMateContext();

        // Get all active announcements
        var listOfAnnouncements = context.Announcements.Where(x => x.IsActive == true).ToList();

        // If there are no active announcements, return
        if (listOfAnnouncements.Count == 0)
        {
            Console.WriteLine("There are no active announcements.");
            _ = Console.ReadLine();
            return;
        }

        // Print all active announcements
        Console.WriteLine("List of active announcements:");
        foreach (var announcement in listOfAnnouncements)
        {
            Console.WriteLine($"Id: {announcement.Id}, Title: {announcement.Title}, Description: {announcement.Description}");
        }

        Console.WriteLine("Enter announcement Id:");
        _ = int.TryParse(Console.ReadLine(), out var announcementId);

        // Get announcement by Id and isActive == true
        var existingAnnouncement = context.Announcements.FirstOrDefault(x => x.Id == announcementId && x.IsActive == true);

        // If announcement does not exist, return
        if (existingAnnouncement == null)
        {
            Console.WriteLine($"Announcement with Id {announcementId} does not exist.");
            _ = Console.ReadLine();
            return;
        }

        // Set isActive to false
        existingAnnouncement.IsActive = false;

        Console.WriteLine($"Announcements {existingAnnouncement.Id}, terminated.");

        // Save changes
        _ = context.SaveChanges();
    }
}
