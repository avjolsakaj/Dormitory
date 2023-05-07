using Dormitory.BLL;

namespace Dormitory;

internal class Program
{
    static void Main (string[] args)
    {
        while (true)
        {
            Console.WriteLine("New announcement (AA)");
            Console.WriteLine("Terminate announcement (TA)");
            Console.WriteLine("Edit announcement (EA)");
            Console.WriteLine("New Application (ADAPP)");
            Console.WriteLine("New Application (APAPP)");
            Console.WriteLine("Exit (Esc)");

            Console.WriteLine("Enter your choice:");
            var choice = Console.ReadLine();

            switch (choice?.ToUpper())
            {
                case "AA":
                    AnnouncementManagement.AddAnnouncement();
                    break;
                case "TA":
                    AnnouncementManagement.TerminateAnnouncement();
                    break;
                case "EA":
                    AnnouncementManagement.EditAnnouncement();
                    break;
                case "ADAPP":
                    ApplicationManagement.AddApplication();
                    break;
                case "APAPP":
                    ApplicationManagement.ApproveApplication();
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
}