using ClassRoom.Models.DataCreate;
using System.ComponentModel.DataAnnotations.Schema;

namespace classroombooking.DataCreate
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Code { get; set; }

        public int? Credits { get; set; }

        public int? LecturerId { get; set; }
        public virtual Lecturer? Lecturers { get; set; }

        [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;



    }
}
