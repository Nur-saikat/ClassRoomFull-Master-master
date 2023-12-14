using ClassRoom.Models.DataCreate;
using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.Room_Booking
{
    public class Routine
    {
        public int Id { get; set; }

     
        [Display(Name = "Session")]
        public int? SessionId { get; set; }
        public virtual Session? Sessions { get; set; }


        [Display(Name = "Lecturer")]
        public int? LecturerId { get; set; }
        public virtual Lecturer? Lecturers { get; set; }

        [Display(Name = "Course")]
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }

        public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();


    }
}
