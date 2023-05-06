using Dormitory.BLL;

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
                    AnnouncementManagement.AddAnnouncemnt();
                    break;
                case "EA":
                    AnnouncementManagement.TerminateAnnouncemnt();
                    break;
                case "AAPP":
                    ApplicationManagement.AddApplication();
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