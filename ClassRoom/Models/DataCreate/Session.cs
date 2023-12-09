using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.DataCreate
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Session_Start_Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime Session_End_Date { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; } = new List<StudentCourse>();
        public virtual ICollection<LecturerCourse> LecturerCourses { get; } = new List<LecturerCourse>();
    }
}
