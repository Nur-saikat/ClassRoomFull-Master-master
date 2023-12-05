using classroombooking.DataCreate;

namespace ClassRoom.Models.DataCreate
{
    public class StudentCourse
    {
        public int Id { get; set; } 
        public int? StudentId { get; set; }
        public virtual Student? Students { get; set; }

        public int? CourseId { get; set; }
        public virtual Course? Courses { get; set; }

        public int? SessionId { get; set; }
        public virtual Session? Sessions { get; set; }

    }
}
