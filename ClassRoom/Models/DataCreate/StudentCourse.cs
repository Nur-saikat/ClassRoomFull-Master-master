using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.DataCreate
{
    public class StudentCourse
    {
        public int Id { get; set; }
        [Display(Name = "Student Id")]
        public int? StudentId { get; set; }
        public virtual Student? Students { get; set; }
        [Display(Name = "Course Id")]
        public int? CourseId { get; set; }
        public virtual Course? Courses { get; set; }

    }
}
