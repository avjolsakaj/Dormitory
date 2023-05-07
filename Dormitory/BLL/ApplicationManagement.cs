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
            return;
        }

        // Check if student has other active applications for this announcement
        var applicationExists = context.Applications
            .Any(x => x.AnnouncementId == announcementId && x.StudentId == studentId && x.IsActive == true);

        if (applicationExists)
        {
            Console.WriteLine($"Student {studentId}, already has an active application for announcement {announcementId}!");
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

    public static void ApproveApplication ()
    {
        using var context = new FindRooMateContext();

        // Get all active applications
        var applications = context.Applications
            .Include(x => x.Announcement)
                .ThenInclude(x => x.Room)
            .Include(x => x.Student)
            .Where(x => x.IsActive == true)
            .ToList();

        // Show all applications to user
        Console.WriteLine("List of applications:");
        foreach (var application in applications)
        {
            Console.WriteLine($"Id: {application.Id}, Announcement: {application.Announcement.Title}, " +
                               $"Student Id: {application.StudentId}, Student: {application.Student.Name} {application.Student.Surname}, " +
                               $"Room Id: {application.Announcement.RoomId}, Room Name: {application.Announcement.Room.Name}");
        }

        // Get application Id
        Console.WriteLine("Give me application Id:");
        _ = int.TryParse(Console.ReadLine(), out var applicationId);

        // Check if application exists
        var applicationToApprove = applications.FirstOrDefault(application => application.Id == applicationId);

        if (applicationToApprove == null)
        {
            Console.WriteLine($"Application {applicationId}, does not exist!");
            return;
        }

        // Get student id from application
        var studentId = applicationToApprove.StudentId;

        // Get room id from application
        var roomId = applicationToApprove.Announcement.RoomId;

        // Check if max number of students is reached for this room
        var countNumberOfStudentsPerRoom = context.RoomStudents.Count(x =>
            x.RoomId == applicationToApprove.Announcement.RoomId && x.EndDate != null);
        // && (x.EndDate != null || x.EndDate > DateTime.Now.AddDays(1)));

        // Get max number of students for this room
        var maxCapacity = applicationToApprove.Announcement.Room.Capacity;

        if (countNumberOfStudentsPerRoom >= maxCapacity)
        {
            Console.WriteLine("This room can not accept more students!");
            return;
        }

        // Add to RoomStudent
        _ = context.RoomStudents.Add(new RoomStudent
        {
            RoomId = roomId,
            StudentId = studentId,
            StartDate = DateTime.Now
        });

        // Show message to user that application is approved
        Console.WriteLine($"Application {applicationId} is approved. And Student {studentId}, is in Room {roomId}");

        // Get all active applications for this announcement
        var appActiveApplicationsForAnnouncemnt = context.Applications
            .Where(x => x.IsActive == true && x.AnnouncementId == applicationToApprove.AnnouncementId)
            .ToList();

        // Set other application, for this room, to inactive
        foreach (var otherApp in appActiveApplicationsForAnnouncemnt)
        {
            // Make application inactive
            otherApp.IsActive = false;
        }

        // The other implementation for the above foreach
        // appActiveApplicationsForAnnouncemnt.ForEach(x => x.IsActive = false);

        // Set this announcement to inactive
        applicationToApprove.Announcement.IsActive = false;

        // Save changes
        _ = context.SaveChanges();
    }
}
