using API.DAL;
using API.DTO;

namespace API.Converters;

public static class Convert
{
    public static AnnouncementDTO Map (this Announcement entity)
    {
        return new AnnouncementDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            PublishedDate = entity.PublishedDate,
            Room = entity.Room == null ? null : new RoomDTO
            {
                // When room is nullable, check for it like this: Room?
                Id = entity.Room.Id,
                Name = entity.Room.Name
            }
        };
    }
}
