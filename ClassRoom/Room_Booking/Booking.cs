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
        [Display(Name = "Class Start")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Class End")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime Finish { get; set; }

        [Display(Name = "Lecturer")]
        public int? LecturerId { get; set; }
        public virtual Lecturer? Lecturers { get; set; }
        [Display(Name = "Room")]
        public int? RoomId { get; set; }
        public virtual Room? Rooms { get; set; }
        [Display(Name = "Course")]
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }

        internal static object Select(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
