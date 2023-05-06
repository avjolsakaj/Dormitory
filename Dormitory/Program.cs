using Dormitory.DAL;

namespace Dormitory;

internal class Program
{
    static void Main (string[] args)
    {
        while (true)
        {
            Console.WriteLine("New announcement (AA)");
            Console.WriteLine("Edit announcement (EA)");
            Console.WriteLine("New Application (AAPP)");
            Console.WriteLine("Exit (Esc)");

            Console.WriteLine("Enter your choice:");
            var choice = Console.ReadLine();

            switch (choice?.ToUpper())
            {
                case "AA":
                    AddAnnouncemnt();
                    break;
                case "EA":
                    //TerminateAnnouncemnt();
                    break;
                case "AAPP":
                    //NewApplication();
                    break;
                case "ESC":
                    Console.WriteLine("Bye Bye");
                    return;
                default:
                    Console.WriteLine("Choice is not valid.");
                    break;
            }
        }
    }

    private static void AddAnnouncemnt ()
    {
        using var context = new FindRooMateContext();

        Console.WriteLine("Enter announcement title:");
        var title = Console.ReadLine();

        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine("Title is required.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Enter announcement description:");
        var description = Console.ReadLine();

        if (string.IsNullOrEmpty(description))
        {
            Console.WriteLine("Description is required.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("List of rooms: ");
        var existingRooms = context.Rooms.ToList();
        foreach (var room in existingRooms)
        {
            Console.WriteLine($"Id: {room.Id}, Room: {room.Name}, Capacity: {room.Capacity}");
        }

        Console.WriteLine("Enter room Id:");
        int.TryParse(Console.ReadLine(), out var roomId);

        var roomExists = existingRooms.Any(room => room.Id == roomId);
        if (!roomExists)
        {
            Console.WriteLine($"Room {roomId}, does not exist!");
            Console.ReadLine();
            return;
        }

        var hasActiveAccouncemntForRoom = context.Announcements
            .Any(x => x.IsActive == true && x.RoomId == roomId);

        if (hasActiveAccouncemntForRoom)
        {
            Console.WriteLine("There is already active announcement for this room.");
            Console.ReadLine();
            return;
        }

        _ = context.Announcements.Add(new Announcement
        {
            Title = title,
            Description = description,
            PublishedDate = DateTime.Now,
            IsActive = true,
            RoomId = roomId
        });

        _ = context.SaveChanges();
    }
}