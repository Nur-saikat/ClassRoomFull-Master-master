using classroombooking.DataCreate;

namespace ClassRoom.Models.DataCreate
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; } = new List<StudentCourse>();
    }
}
