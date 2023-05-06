using Dormitory.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.BLL;

public class ApplicationManagement
{
    public static void AddApplication ()
    {
        using var context = new FindRooMateContext();

        // Get all announcements
        var existingAnnouncements = context.Announcements
            .Include(x => x.Room)
            .Where(x => x.IsActive == true)
            .ToList();

        // Show all announcements
        Console.WriteLine("List of announcements: ");
        foreach (var announcement in existingAnnouncements)
        {
            Console.WriteLine($"Id: {announcement.Id}, Title: {announcement.Title}, " +
                $"Description: {announcement.Description}, Room: {announcement.Room.Name}");
        }

        // Get announcement Id
        Console.WriteLine("Enter announcement Id:");
        _ = int.TryParse(Console.ReadLine(), out var announcementId);

        // Check if announcement exists
        var announcementExists = existingAnnouncements.Any(announcement => announcement.Id == announcementId);

        if (!announcementExists)
        {
            Console.WriteLine($"Announcement {announcementId}, does not exist!");
            _ = Console.ReadLine();
            return;
        }

        // Get list of students
        var existingStudents = context.Students.ToList();

        // Show all students
        Console.WriteLine("List of students: ");
        foreach (var student in existingStudents)
        {
            Console.WriteLine($"Id: {student.Id}, Name: {student.Name}, Surname: {student.Surname}");
        }

        // Get student Id
        Console.WriteLine("Enter student Id:");
        _ = int.TryParse(Console.ReadLine(), out var studentId);

        // Check if student exists
        var studentExists = existingStudents.Any(student => student.Id == studentId);

        if (!studentExists)
        {
            Console.WriteLine($"Student {studentId}, does not exist!");
            _ = Console.ReadLine();
            return;
        }

        // Create application
        var application = new Application
        {
            AnnouncementId = announcementId,
            StudentId = studentId,
            ApplicationDate = DateTime.Now,
            IsActive = true
        };

        // Add application to database
        var created = context.Applications.Add(application);

        Console.WriteLine($"Application {created.Entity.Id} is added.");

        // Save changes
        _ = context.SaveChanges();
    }
}
