using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.DataCreate
{
    public class LecturerCourse
    {
        public int Id { get; set; }
        [Display(Name = "Lecturer Name")]
        public int? LecturerId { get; set; }
        public virtual Lecturer? Lecturers { get; set; }
        [Display(Name = "Course Name")]
        public int? CourseId { get; set; }
        public virtual Course? Courses { get; set; }
    }
}
