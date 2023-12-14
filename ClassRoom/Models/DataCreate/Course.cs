using ClassRoom.Models.DataCreate;
using ClassRoom.Models.Room_Booking;
using System.ComponentModel.DataAnnotations.Schema;

namespace classroombooking.DataCreate
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Code { get; set; }

        public int? Credits { get; set; }

    
        [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;

        public virtual ICollection<StudentCourse> StudentCourses { get; } = new List<StudentCourse>();
        public virtual ICollection<LecturerCourse> LecturerCourses { get; } = new List<LecturerCourse>();
        public virtual ICollection<Routine> Routines { get; } = new List<Routine>();

    }
}
