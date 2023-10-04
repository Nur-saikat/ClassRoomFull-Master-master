//using ClassRoom.DataCreate;

using ClassRoom.Models.DataCreate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Room = ClassRoom.Models.Room;

namespace classroombooking.DataCreate
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

        public int LecturerId { get; set; } 
        public virtual Lecturer? Lecturers { get; set; }
       
        public int RoomId { get; set; }
        public virtual Room? Rooms { get; set; }

        [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;

    }
}
